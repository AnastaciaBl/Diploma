using Diploma.Algorithms.Distribution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Algorithms.EM
{
    public class SEMAlgorithm: EMAlgorithm
    {
        public SEMAlgorithm(int amountOfClusters, IDistribution distribution, List<double> values, double eps) : base(
            amountOfClusters, distribution, values, eps)
        {
            
        }
    }
}
