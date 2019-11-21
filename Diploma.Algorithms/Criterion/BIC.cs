using Diploma.Algorithms.Distribution;
using System;

namespace Diploma.Algorithms.Criterion
{
    public class BIC
    {
        public static double Count(IDistribution distribution, int paramAmount, double x, int observationAmount, params double[] paramVector)
        {
            var result = Math.Log10(observationAmount) * paramAmount -
                     2 * Math.Log10(distribution.CountProbabilityFunctionResult(paramVector[0], paramVector[1], x));
            return Math.Round(result, 5);
        }
    }
}