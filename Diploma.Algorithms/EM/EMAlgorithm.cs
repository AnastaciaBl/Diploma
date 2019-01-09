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
            //public double Probability;

            public Parameters(double m, double g)//, double probability)
            {
                MStruct = m;
                GStruct = g;
                //Probability = probability;
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
                //var probability = Distribution.CountProbabilityFunctionResult(m, g)
                HiddenVector.Add(new Parameters(m, g));
            }
        }
    }
}
