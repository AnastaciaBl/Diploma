using System;

namespace Diploma.Algorithms.Distribution
{
    class NormalDistribution : IDistribution
    {
        public double M { get; set; }
        public double G { get; set; }

        public double CountProbabilityFunctionResult(double xValue)
        {
            return (1.0 / (G * Math.Sqrt(2 * Math.PI))) * Math.Exp(-1.0 * (Math.Pow(xValue - M, 2) / (2 * G * G)));
        }
    }
}
