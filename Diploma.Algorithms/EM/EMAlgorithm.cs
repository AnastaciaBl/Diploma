using System;
using System.Collections.Generic;
using System.Linq;
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

        public EMAlgorithm(int amountOfClusters, IDistribution distribution, List<double> values)
        {
            AmountOfClusters = amountOfClusters;
            Distribution = distribution;
            DataSetValues = values;
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
            var probabilityInPoint = CountProbabilitiesForEachPoint();
            var averages = CountAverage(probabilityInPoint);
            var dispersions = CountDispersion(averages, probabilityInPoint);
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

        private double[] CountAverage(double[,] probabilities)
        {
            var averages = new double[AmountOfClusters];
            for (int i = 0; i < AmountOfClusters; i++)
            {
                double sumUp = 0;
                for (int j = 0; j < DataSetValues.Count; j++)
                {
                    sumUp += probabilities[i, j] * DataSetValues[j];
                }

                averages[i] = sumUp / CountSumOfProbabilitiesInCluster(i, probabilities);
            }
            return averages;
        }

        private double[] CountDispersion(double[] averages, double[,] probabilities)
        {
            var dispersions = new double[AmountOfClusters];
            for (int i = 0; i < AmountOfClusters; i++)
            {
                double sum = 0;
                for (int j = 0; j < DataSetValues.Count; j++)
                {
                    sum += probabilities[i, j] * Math.Pow(DataSetValues[j] - averages[i], 2);
                }

                dispersions[i] = sum / CountSumOfProbabilitiesInCluster(i, probabilities);
            }

            return dispersions;
        }

        private double CountSumOfProbabilitiesInCluster(int index, double[,] probabilities)
        {
            double sum = 0;
            for (int j = 0; j < DataSetValues.Count; j++)
            {
                sum += probabilities[index, j];
            }

            return sum;
        }
    }
}
