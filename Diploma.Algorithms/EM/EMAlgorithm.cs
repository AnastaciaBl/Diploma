using Diploma.Algorithms.Distribution;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
        public List<Parameters> HiddenVector { get; protected set; }

        public EMAlgorithm(IDistribution distribution, List<double> values, double eps)
        {
            AmountOfElements = values.Count;
            Distribution = distribution;
            DataSetValues = values;
            Labels = new int[values.Count];
            Eps = eps;
        }

        public virtual void SplitOnClusters(int amountOfClusters)
        {
            try
            {
                AmountOfClusters = amountOfClusters;
                Probabilities = new double[AmountOfElements, AmountOfClusters];

                Random random = new Random(2);
                FillProbabilityMatrixByRandomValues(random);
                var oldProbabilitiesMatrix = new double[AmountOfElements, AmountOfClusters];
                var index = 0;
                do
                {
                    Array.Copy(Probabilities, oldProbabilitiesMatrix, AmountOfClusters * AmountOfElements);
                    MStep();
                    EStep();
                    index++;
                } while (CountChangesInProbabilitiesMatrix(oldProbabilitiesMatrix) > Eps && (index < 500));

                SetUpLabels();

                if (Labels.First() == -1 || Double.IsNaN(HiddenVector.First().MStruct))
                    SplitOnClusters(amountOfClusters);
            }
            catch
            {
                SplitOnClusters(amountOfClusters);
            }
        }

        protected void FillProbabilityMatrixByRandomValues(Random random)
        {
            //we need to check that sum of probabilities less than 1 and fix this if it is not
            for (int i = 0; i < AmountOfElements; i++)
            {
                double sum = 0;
                for (int j = 0; j < AmountOfClusters; j++)
                {
                    Probabilities[i, j] = random.NextDouble();
                    sum += Probabilities[i, j];
                }
                for (int j = 0; j < AmountOfClusters; j++)
                    Probabilities[i, j] = Probabilities[i, j] / sum;
            }
        }

        protected virtual void EStep()
        {
            Probabilities = CountProbabilitiesForEachPoint();
            SetUpLabels();
        }

        protected virtual void MStep()
        {
            var averages = CountAverage();
            var dispersions = CountDispersion(averages);
            var probabilitiesToBeInCluster = CountProbabilitiesToBeInCluster();
            UpdateParameters(averages, dispersions, probabilitiesToBeInCluster);
        }

        //TODO some problems with dispersion
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
                    var probability = HiddenVector[j].СStruct * Distribution.CountProbabilityFunctionResult(HiddenVector[j].MStruct,
                                              HiddenVector[j].GStruct, DataSetValues[i]);
                    probabilities[i, j] = probability / sum;
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
            int z = 0, f = 0, s = 0;
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
                if (cluster == 0)
                    z++;
                else if (cluster == 1)
                    f++;
                else s++;
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

        private void PrintProbabilities(string fileName)
        {
            using (var sw = new StreamWriter(fileName, true))
            {
                for (int i = 0; i < AmountOfElements; i++)
                {
                    string sr = string.Empty;
                    for (int j = 0; j < AmountOfClusters; j++)
                        sr += Probabilities[i, j] + "\t";
                    sw.WriteLine(sr);
                }
                sw.WriteLine("**************************************************************************************************");
            }
        }

        private void PrintParameters(string fileName)
        {
            using (var sw = new StreamWriter(fileName, true))
            {
                for (int i = 0; i < AmountOfClusters; i++)
                {
                    string sr = string.Empty;
                    sr = "M: " + HiddenVector[i].MStruct + "\tG: " + HiddenVector[i].GStruct + "\tC: " +
                         HiddenVector[i].СStruct;
                    sw.WriteLine(sr);
                }
                sw.WriteLine("**************************************************************************************************");
            }
        }
    }
}