using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Statistics.Analysis;

namespace Diploma.Algorithms.PCA
{
    public class PCA
    {
        public double[][] DataSet { get; set; }
        public int[] Labels { get; set; }

        public PCA(List<double> data)
        {
            DataSet = new double[data.Count][];
            for (var i = 0; i < data.Count; i++)
                DataSet[i] = new[] { data[i], 2.5 };
        }

        public double[][] MakeComponents()
        {
            var pca = new PrincipalComponentAnalysis(PrincipalComponentMethod.Center);
            pca.Learn(DataSet);
            var components = pca.CumulativeProportions;
            var vectors = pca.ComponentVectors;
            var proportion = pca.Components[0].CumulativeProportion * 100;
            
            return pca.Transform(DataSet); //projection
        }
    }
}
