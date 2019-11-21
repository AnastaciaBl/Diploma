using Accord.Statistics.Analysis;
using Diploma.Model;

namespace Diploma.Algorithms.PCA
{
    public class PCA
    {
        private readonly PrincipalComponentAnalysis Pca;

        public double[][] DataSet { get; }
        public double[][] ProjectionSet { get; private set; }
        public double[] EigenValues { get; set; }
        public double[] SingularValues { get; set; }
        public double[] Proportion { get; set; }
        public double[] CumulativeProportion { get; set; }
        public int AmountOfComponents { get; private set; }

        public PCA(double[][] data)
        {
            DataSet = data;
            Pca = new PrincipalComponentAnalysis(PrincipalComponentMethod.Center);
        }

        public void CreateComponents()
        {
            Pca.Learn(DataSet);
            ProjectionSet = Pca.Transform(DataSet);
            CumulativeProportion = Pca.CumulativeProportions;
            Proportion = Pca.ComponentProportions;
            FillParameters();
        }

        private void FillParameters()
        {
            AmountOfComponents = Pca.Components.Count;
            EigenValues = new double[AmountOfComponents];
            SingularValues = new double[AmountOfComponents];
            for (var i = 0; i < AmountOfComponents; i++)
            {
                EigenValues[i] = Pca.Components[i].Eigenvalue;
                SingularValues[i] = Pca.Components[i].SingularValue;
            }
        }

        public double[] GetVecorByIndex(int index)
        {
            var data = new double[Constant.AMOUNT_OF_ATTRIBUTES];
            for (var i = 0; i < data.Length; i++)
                data[i] = ProjectionSet[index][i];

            return data;
        }
    }
}