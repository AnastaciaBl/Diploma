using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Diploma.Algorithms.Distribution;

namespace Diploma.Algorithms.EM
{
    public class EMAlgorithm
    {
        public int AmountOfClusters { get; protected set; }
        public readonly IDistribution Distribution;
        public readonly List<double> DataSetValues;
        public readonly double Eps;
        public int[] Labels { get; protected set; }
        protected struct Parameters
        {
            public double MStruct;
            public double GStruct;

            public Parameters(double m, double g)
            {
                MStruct = m;
                GStruct = g;
            }
        }
        protected List<Parameters> HiddenVector { get; set; }
        protected double[,] Probabilities { get; set; }

        public EMAlgorithm(int amountOfClusters, IDistribution distribution, List<double> values, double eps)
        {
            AmountOfClusters = amountOfClusters;
            Distribution = distribution;
            DataSetValues = values;
            Labels = new int[values.Count];
            Eps = eps;
        }

        public void SplitOnClusters()
        {
            FillHiddenVectorByRandomValues();
            double oldParameterM = HiddenVector.First().MStruct;
            double oldParameterG = HiddenVector.First().GStruct;
            do
            {
                EStep();
                MStep();
            } while (Math.Abs(oldParameterM - HiddenVector.First().MStruct) < Eps && Math.Abs(oldParameterG - HiddenVector.First().GStruct) < Eps);
        }

        private void FillHiddenVectorByRandomValues()
        {
            var random = new Random();
            HiddenVector = new List<Parameters>();
            for (int i = 0; i < AmountOfClusters; i++)
            {
                //TODO what interval should be?
                var m = random.Next(5) + 0.1 * random.Next(10);
                var g = random.Next(5) + 0.1 * random.Next(10);
                HiddenVector.Add(new Parameters(m, g));
            }
        }

        private void EStep()
        {
            Probabilities = CountProbabilitiesForEachPoint();
            SetUpLabels();
        }

        protected virtual void MStep()
        {
            var averages = CountAverage();
            var dispersions = CountDispersion(averages);
            UpdateParameters(averages, dispersions);
        }

        private double[,] CountProbabilitiesForEachPoint()
        {
            var probabilities = new double[AmountOfClusters, DataSetValues.Count];
            for (int i = 0; i < DataSetValues.Count; i++)
            {
                for (int j = 0; j < AmountOfClusters; j++)
                {
                    probabilities[j, i] = Distribution.CountProbabilityFunctionResult(HiddenVector[j].MStruct,
                        HiddenVector[j].GStruct, DataSetValues[i]);
                }
            }
            return probabilities;
        }

        private double[] CountAverage()
        {
            var averages = new double[AmountOfClusters];
            for (int i = 0; i < AmountOfClusters; i++)
            {
                double sumUp = 0;
                for (int j = 0; j < DataSetValues.Count; j++)
                {
                    sumUp += Probabilities[i, j] * DataSetValues[j];
                }

                averages[i] = sumUp / CountSumOfProbabilitiesInCluster(i);
            }
            return averages;
        }

        private double[] CountDispersion(double[] averages)
        {
            var dispersions = new double[AmountOfClusters];
            for (int i = 0; i < AmountOfClusters; i++)
            {
                double sum = 0;
                for (int j = 0; j < DataSetValues.Count; j++)
                {
                    sum += Probabilities[i, j] * Math.Pow(DataSetValues[j] - averages[i], 2);
                }

                dispersions[i] = sum / CountSumOfProbabilitiesInCluster(i);
            }

            return dispersions;
        }

        private double CountSumOfProbabilitiesInCluster(int index)
        {
            double sum = 0;
            for (int j = 0; j < DataSetValues.Count; j++)
            {
                sum += Probabilities[index, j];
            }

            return sum;
        }

        private void SetUpLabels()
        {
            for (int i = 0; i < DataSetValues.Count; i++)
            {
                int cluster = -1;
                double maxProbability = -1;
                for (int j = 0; j < AmountOfClusters; j++)
                {
                    if (maxProbability < Probabilities[j, i])
                    {
                        maxProbability = Probabilities[j, i];
                        cluster = j;
                    }
                }

                Labels[i] = cluster;
            }
        }

        private void UpdateParameters(double[] averages, double[] dispersions)
        {
            HiddenVector = new List<Parameters>();
            for (int i = 0; i < AmountOfClusters; i++)
            {
                HiddenVector.Add(new Parameters(averages[i], dispersions[i]));
            }
        }
    }
}
