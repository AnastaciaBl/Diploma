using Accord.Statistics;
using Accord.Statistics.Analysis;
using Diploma.Model;
using System;
using System.Collections.Generic;
using Accord.Statistics.Testing;

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
            Pca = new PrincipalComponentAnalysis(PrincipalComponentMethod.Standardize);
        }

        public void CreateComponents()
        {
            Pca.Learn(DataSet);
            ProjectionSet = Pca.Transform(DataSet);
            CumulativeProportion = new double[Pca.CumulativeProportions.Length];
            Proportion = new double [Pca.ComponentProportions.Length];
            for (var i = 0; i < Proportion.Length; i++)
            {
                CumulativeProportion[i] = Math.Round(Pca.CumulativeProportions[i], 3);
                Proportion[i] = Math.Round(Pca.ComponentProportions[i], 3);
            }
            FillParameters();
        }

        private void FillParameters()
        {
            AmountOfComponents = Pca.Components.Count;
            EigenValues = new double[AmountOfComponents];
            SingularValues = new double[AmountOfComponents];
            for (var i = 0; i < AmountOfComponents; i++)
            {
                EigenValues[i] = Math.Round(Pca.Components[i].Eigenvalue, 3);
                SingularValues[i] = Math.Round(Pca.Components[i].SingularValue, 3);
            }
        }

        public double[] GetVecorByIndex(int index)
        {
            var data = new double[Constant.AMOUNT_OF_ATTRIBUTES];
            for (var i = 0; i < data.Length; i++)
                data[i] = ProjectionSet[index][i];

            return data;
        }

        public int[][] GetValuableParameterIndexes(List<int> valuableComponentIndexes, int amountOfObservations)
        {
            var coefficient = 0.5;
            var matrix = new int[valuableComponentIndexes.Count][];
            for (var i = 0; i < valuableComponentIndexes.Count; i++)
            {
                var indexes = new List<int>();
                for (var j = 0; j < Constant.AMOUNT_OF_ATTRIBUTES; j++)
                {
                    var array = GetArrayForCorrelationAnalysis(amountOfObservations, j, valuableComponentIndexes[i]);
                    if (CountCorrelation(array) > coefficient)
                    {
                        if(TTest(amountOfObservations, j, valuableComponentIndexes[i]))
                            indexes.Add(j);
                    }
                }
                matrix[i] = indexes.ToArray();
            }

            return matrix;
        }

        private double[,] GetArrayForCorrelationAnalysis(int amountOfObservations, int parameterIndex, int componentIndex)
        {
            var array = new double[amountOfObservations, 2];
            for (var i = 0; i < amountOfObservations; i++)
            {
                array[i, 0] = DataSet[i][parameterIndex];
                array[i, 1] = ProjectionSet[i][componentIndex];
            }

            return array;
        }

        private double CountCorrelation(double[,] array)
        {
            var matrix = Measures.Correlation(array);
            return Math.Abs(matrix[0, 1]);
        }

        private bool TTest(int amountOfObservations, int parameterIndex, int componentIndex)
        {
            var array1 = new double[amountOfObservations];
            var array2 = new double[amountOfObservations];
            for (var i = 0; i < amountOfObservations; i++)
            {
                array1[i] = DataSet[i][parameterIndex];
                array2[i] = ProjectionSet[i][componentIndex];
            }

            var pairedTTest = new PairedTTest(array1, array2, TwoSampleHypothesis.ValuesAreDifferent);
            return pairedTTest.Significant;
        }
    }
}