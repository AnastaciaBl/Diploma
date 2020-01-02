using MathNet.Numerics;
using System;

namespace Diploma.Algorithms.Criterion
{
    public class RandIndex
    {
        public int[,] CreateMatrixOfLabels(int amountOfClustersEtalon, int amountOfClusters, int[] labelsEtalon, int[] labels)
        {
            var matrix = new int[amountOfClusters + 1, amountOfClustersEtalon + 1];

            for (var i = 0; i < labels.Length; i++)
            {
                var index = labels[i];
                var indexEtalon = labelsEtalon[i];

                matrix[index, indexEtalon]++;
            }

            var rowSum = 0;
            var columnSum = 0;

            for (var i = 0; i < amountOfClusters; i++)
            {
                var sum = 0;
                for (var j = 0; j < amountOfClustersEtalon; j++)
                {
                    sum += matrix[i, j];
                }

                matrix[i, amountOfClustersEtalon] = sum;
                rowSum += sum;
            }

            for (var i = 0; i < amountOfClustersEtalon; i++)
            {
                var sum = 0;
                for (var j = 0; j < amountOfClusters; j++)
                {
                    sum += matrix[j, i];
                }

                matrix[amountOfClusters, i] = sum;
                columnSum += sum;
            }

            if (rowSum == columnSum)
                matrix[amountOfClusters, amountOfClustersEtalon] = rowSum;
            else
                matrix[amountOfClusters, amountOfClustersEtalon] = -1;

            return matrix;
        }

        public double GetRandIndex(int[,] matrix)
        {
            var index = 0.0;
            var rowAmount = matrix.GetLength(0);
            var columnAmount = matrix.GetLength(1);
            double tp_fp = 0.0, tp_fn = 0.0, tp = 0.0, tp_fp_fn_tn = 0.0;

            tp_fp_fn_tn = Combinatorics.Combinations(matrix[rowAmount - 1, columnAmount - 1], 2);
            for (var i = 0; i < rowAmount - 1; i++)
            {
                tp_fp += Combinatorics.Combinations(matrix[i, columnAmount - 1], 2);
                for (var j = 0; j < columnAmount - 1; j++)
                {
                    tp += Combinatorics.Combinations(matrix[i, j], 2);
                }
            }

            for (var i = 0; i < columnAmount - 1; i++)
            {
                tp_fn += Combinatorics.Combinations(matrix[rowAmount - 1, i], 2);
            }

            var fp = tp_fp - tp;
            var tn = tp_fp_fn_tn - tp_fn - fp;

            index = (tp + tn) / tp_fp_fn_tn;
            return Math.Round(index, 3);
        }
    }
}