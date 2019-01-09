using System;

namespace Diploma.Algorithms.Distribution
{
    public class NormalDistribution : IDistribution
    {
        public double M { get; set; }
        public double G { get; set; }

        public NormalDistribution(double m, double g)
        {
            //g - it is already a square number 
            M = m;
            G = g;
        }

        public double CountProbabilityFunctionResult(double xValue)
        {
            var answer = (1.0 / (Math.Sqrt(G) * Math.Sqrt(2 * Math.PI))) *
                         Math.Exp(-1.0 * (Math.Pow(xValue - M, 2) / (2 * G)));
            return Math.Round(answer, 4);
        }
    }
}
