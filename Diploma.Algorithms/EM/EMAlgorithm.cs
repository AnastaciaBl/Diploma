using System;
using System.Collections.Generic;
using Diploma.Algorithms.Distribution;

namespace Diploma.Algorithms.EM
{
    public class EMAlgorithm
    {
        public int AmountOfClusters { get; protected set; }
        public int AmountOfElements { get; }
        public readonly IDistribution Distribution;
        public readonly List<double> DataSetValues;
        public readonly double Eps;
        public int[] Labels { get; protected set; }
        protected double[,] Probabilities { get; set; }
        protected List<Parameters> HiddenVector { get; set; }

        public EMAlgorithm(int amountOfClusters, IDistribution distribution, List<double> values, double eps)
        {
            AmountOfClusters = amountOfClusters;
            AmountOfElements = values.Count;
            Distribution = distribution;
            DataSetValues = values;
            Labels = new int[values.Count];
            Eps = eps;
            Probabilities = new double[AmountOfElements, AmountOfClusters];
        }

        public virtual void SplitOnClusters()
        {
            FillProbabilityMatrixByRandomValues();
            var oldProbabilitiesMatrix = new double[AmountOfElements, AmountOfClusters];
            do
            {
                Array.Copy(Probabilities, oldProbabilitiesMatrix, AmountOfClusters * AmountOfElements);
                MStep();
                EStep();
            } while (CountChangesInProbabilitiesMatrix(oldProbabilitiesMatrix) < Eps);
            SetUpLabels();
        }

        protected void FillProbabilityMatrixByRandomValues()
        {
            var random = new Random();
            //we need to check that sum of probabilities less than 1 and fix this if it is not
            for (int i = 0; i < AmountOfElements; i++)
            {
                double sum = 0;
                for (int j = 0; j < AmountOfClusters; j++)
                {
                    Probabilities[i, j] = random.NextDouble();
                    sum += Probabilities[i, j];
                }
                if (sum > 1)
                {
                    for (int j = 0; j < AmountOfClusters; j++)
                        Probabilities[i, j] = Probabilities[i, j] / sum;
                }
                else if(sum<1)
                {
                    i--;
                }
            }
        }

        protected virtual void EStep()
        {
            Probabilities = CountProbabilitiesForEachPoint();
            //SetUpLabels();
        }

        protected virtual void MStep()
        {
            var averages = CountAverage();
            var dispersions = CountDispersion(averages);
            var probabilitiesToBeInCluster = CountProbabilitiesToBeInCluster();
            UpdateParameters(averages, dispersions, probabilitiesToBeInCluster);
        }

        private double[,] CountProbabilitiesForEachPoint()
        {
            var probabilities = new double[AmountOfElements, AmountOfClusters];
            for (int i = 0; i < AmountOfElements; i++)
            {
                double sum = 0;
                for (int j = 0; j < AmountOfClusters; j++)
                {
                    sum += HiddenVector[j].СStruct * Distribution.CountProbabilityFunctionResult(
                               HiddenVector[j].MStruct,
                               HiddenVector[j].GStruct, DataSetValues[i]);
                }
                for (int j = 0; j < AmountOfClusters; j++)
                {
                    probabilities[i, j] = HiddenVector[j].СStruct * Distribution.CountProbabilityFunctionResult(HiddenVector[j].MStruct,
                        HiddenVector[j].GStruct, DataSetValues[i]) / sum;
                }
            }
            return probabilities;
        }

        protected virtual double[] CountAverage()
        {
            var averages = new double[AmountOfClusters];
            for (int i = 0; i < AmountOfClusters; i++)
            {
                double sumUp = 0;
                for (int j = 0; j < AmountOfElements; j++)
                {
                    sumUp += Probabilities[j, i] * DataSetValues[j];
                }

                averages[i] = sumUp / CountSumOfProbabilitiesInCluster(i);
            }
            return averages;
        }

        protected virtual double[] CountDispersion(double[] averages)
        {
            var dispersions = new double[AmountOfClusters];
            for (int i = 0; i < AmountOfClusters; i++)
            {
                double sum = 0;
                for (int j = 0; j < AmountOfElements; j++)
                {
                    sum += Probabilities[j, i] * Math.Pow(DataSetValues[j] - averages[i], 2);
                }

                dispersions[i] = sum / CountSumOfProbabilitiesInCluster(i);
            }

            return dispersions;
        }

        protected virtual double CountSumOfProbabilitiesInCluster(int index)
        {
            double sum = 0;
            for (int j = 0; j < AmountOfElements; j++)
            {
                sum += Probabilities[j, index];
            }

            return sum;
        }

        protected virtual double[] CountProbabilitiesToBeInCluster()
        {
            var cValues = new double[AmountOfClusters];
            for (int i = 0; i < AmountOfClusters; i++)
            {
                cValues[i] = CountSumOfProbabilitiesInCluster(i) / AmountOfElements;
            }

            return cValues;
        }

        protected virtual void SetUpLabels()
        {
            for (int i = 0; i < AmountOfElements; i++)
            {
                int cluster = -1;
                double maxProbability = -1;
                for (int j = 0; j < AmountOfClusters; j++)
                {
                    if (maxProbability < Probabilities[i, j])
                    {
                        maxProbability = Probabilities[i, j];
                        cluster = j;
                    }
                }

                Labels[i] = cluster;
            }
        }

        private void UpdateParameters(double[] averages, double[] dispersions, double[] probabilitiesToBeInCluster)
        {
            HiddenVector = new List<Parameters>();
            for (int i = 0; i < AmountOfClusters; i++)
            {
                HiddenVector.Add(new Parameters(averages[i], dispersions[i], probabilitiesToBeInCluster[i]));
            }
        }

        protected double CountChangesInProbabilitiesMatrix(double[,] oldProbabilities)
        {
            double difference = 0;
            for (int i = 0; i < AmountOfElements; i++)
            {
                for (int j = 0; j < AmountOfClusters; j++)
                {
                    var dif = Math.Abs(oldProbabilities[i, j] - Probabilities[i, j]);
                    if (dif > difference)
                        difference = dif;
                }
            }
            return difference;
        }
    }
}
