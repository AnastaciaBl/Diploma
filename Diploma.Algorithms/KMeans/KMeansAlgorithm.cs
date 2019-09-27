using Accord.MachineLearning;

namespace Diploma.Algorithms.Kmeans
{
    public class KMeansAlgorithm
    {
        public int AmountOfClusters { get; set; }
        public double[][] DataSet { get; set; }
        public int[] Labels { get; set; }

        public KMeansAlgorithm(int amountOfClusters, double[] data)
        {
            AmountOfClusters = amountOfClusters;
            DataSet = new[]
            {
                data
            };
        }

        public void SplitOnClusters()
        {
            var kMeans = new KMeans(AmountOfClusters);
            var clusters = kMeans.Learn(DataSet);
            Labels = clusters.Decide(DataSet);
        }
    }
}
