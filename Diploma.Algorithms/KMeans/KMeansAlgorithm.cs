using Accord.MachineLearning;

namespace Diploma.Algorithms.Kmeans
{
    public class KMeansAlgorithm
    {
        public int[] SplitOnClusters(int amountOfClusters, double[][] data)
        {
            var kMeans = new KMeans(amountOfClusters);
            var clusters = kMeans.Learn(data);
            return clusters.Decide(data);
        }
    }
}