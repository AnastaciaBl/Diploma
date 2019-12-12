using Diploma.Algorithms.Distribution;
using System;
using System.Collections.Generic;
using Diploma.Algorithms.EM;

namespace Diploma.Algorithms.Criterion
{
    public class BIC
    {
        public static double Count(IDistribution distribution, int paramAmount, List<double> dataSet, List<Parameters> hiddenVector)
        {
            double lgSum = 0;
            for (var i = 0; i < dataSet.Count; i++)
            {
                double fSum = 0;
                foreach (var hv in hiddenVector)
                {
                    fSum += hv.СStruct *
                            distribution.CountProbabilityFunctionResult(hv.MStruct, hv.GStruct, dataSet[i]);
                }

                lgSum += Math.Log10(fSum);
            }
            var result = Math.Log10(dataSet.Count) * paramAmount - 2 * lgSum;
            return Math.Round(result, 4);
        }
    }
}