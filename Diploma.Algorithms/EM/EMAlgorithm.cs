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
            var probabilities = CountProbabilitiesForEachPoint();

        }

        private double[,] CountProbabilitiesForEachPoint()
        {
            var probabilities = new double[DataSetValues.Count, AmountOfClusters];
            for (int i = 0; i < DataSetValues.Count; i++)
            {
                for (int j = 0; j < AmountOfClusters; j++)
                {
                    probabilities[i, j] = Distribution.CountProbabilityFunctionResult(HiddenVector[j].MStruct,
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
                double sumDown = 0;
                for (int j = 0; j < DataSetValues.Count; j++)
                {
                    sumUp += probabilities[j, i] * DataSetValues[j];
                    sumDown += probabilities[j, i];
                }
                //TODO have to finished a method
                //averages[i] = 
            }

            return averages;
        }
    }
}
