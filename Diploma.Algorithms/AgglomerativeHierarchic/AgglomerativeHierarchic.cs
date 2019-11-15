namespace Diploma.Algorithms.AgglomerativeHierarchic
{
    public class AgglomerativeHierarchic
    {
        public double[,] DataSet { get; }
        public int AmountOfAttributes { get; set; }
        public int AmountOfElements { get; set; }
        public int[][] Clusters { get; set; }

        public AgglomerativeHierarchic(double[][] data, int amountOfAttributes, int amountOfElements)
        {
            AmountOfAttributes = amountOfAttributes;
            AmountOfElements = amountOfElements;

            Clusters = new int[AmountOfElements][];
            DataSet = new double[AmountOfElements, AmountOfAttributes];
            for (var i = 0; i < AmountOfElements; i++)
            {
                for (var j = 0; j < AmountOfAttributes; j++)
                    DataSet[i, j] = data[i][j];
            }
        }

        public void SplitOnClusters()
        {
            var euclideanDistance = 2;
            alglib.ahcreport report;
            alglib.clusterizerstate clustersState;
            alglib.clusterizercreate(out clustersState);

            alglib.clusterizersetpoints(clustersState, DataSet, euclideanDistance);
            alglib.clusterizerrunahc(clustersState, out report);
            FillClusters(report);
        }

        private void FillClusters(alglib.ahcreport report)
        {
            for (var i = 0; i < AmountOfElements; i++)
            {
                int[] elements, cz;
                alglib.clusterizergetkclusters(report, i + 1, out elements, out cz);
                Clusters[i] = new int[elements.Length];
                for (var j = 0; j < elements.Length; j++)
                    Clusters[i][j] = elements[j];
            }
        }
    }
}