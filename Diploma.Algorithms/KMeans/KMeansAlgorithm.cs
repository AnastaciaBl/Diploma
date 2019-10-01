using System.Collections.Generic;
using Accord.MachineLearning;

namespace Diploma.Algorithms.Kmeans
{
    public class KMeansAlgorithm
    {
        public int AmountOfClusters { get; set; }
        public double[][] DataSet { get; set; }
        public int[] Labels { get; set; }

        public KMeansAlgorithm(int amountOfClusters, List<double> data)
        {
            AmountOfClusters = amountOfClusters;
            DataSet = new double[data.Count][];
            for (var i = 0; i < data.Count; i++)
                DataSet[i] = new [] {data[i], 0};
        }

        public KMeansAlgorithm(int amountOfClusters, double[][] data)
        {
            AmountOfClusters = amountOfClusters;
            DataSet = data;
        }

        public void SplitOnClusters()
        {
            var kMeans = new KMeans(AmountOfClusters);
            var clusters = kMeans.Learn(DataSet);
            Labels = clusters.Decide(DataSet);
        }
    }
}
