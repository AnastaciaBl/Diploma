using MathNet.Numerics.Distributions;
using System;

namespace Diploma.Algorithms.Distribution
{
    public class NormalDistribution : IDistribution
    {
        //shift factor - M
        //scale factor - G 

        public double CountProbabilityFunctionResult(double m, double g, double xValue)
        {
            var norm = new Normal(m, Math.Sqrt(g));
            var answer = norm.Density(xValue);
            return answer;
        }
    }
}