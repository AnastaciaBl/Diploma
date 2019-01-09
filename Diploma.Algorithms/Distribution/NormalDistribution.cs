using System;

namespace Diploma.Algorithms.Distribution
{
    public class NormalDistribution : IDistribution
    {
        //shift factor - M
        //scale factor - G (it is already a square number )

        public double CountProbabilityFunctionResult(double m, double g, double xValue)
        {
            var answer = (1.0 / (Math.Sqrt(g) * Math.Sqrt(2 * Math.PI))) *
                         Math.Exp(-1.0 * (Math.Pow(xValue - m, 2) / (2 * g)));
            return Math.Round(answer, 4);
        }
    }
}
