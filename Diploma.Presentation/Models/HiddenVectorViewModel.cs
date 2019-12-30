using System;
using System.Collections.Generic;
using Diploma.Algorithms.EM;

namespace Diploma.Presentation.Models
{
    public class HiddenVectorViewModel
    {
        public double Average { get; set; }
        public double Dispersion { get; set; }
        public double Probability { get; set; }

        public static List<HiddenVectorViewModel> GetListOfHiddenVectorVM(List<Parameters> parameterses, int amountOfClusters)
        {
            var hv = new List<HiddenVectorViewModel>();
            for (var i = 0; i < amountOfClusters; i++)
            {
                hv.Add(new HiddenVectorViewModel
                {
                    Average = Math.Round(parameterses[i].MStruct, 3),
                    Dispersion = Math.Round(parameterses[i].GStruct, 3),
                    Probability = Math.Round(parameterses[i].СStruct, 3)
                });
            }

            return hv;
        }
    }
}