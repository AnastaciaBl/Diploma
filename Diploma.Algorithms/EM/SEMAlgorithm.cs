using Diploma.Algorithms.Distribution;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Diploma.Algorithms.EM
{
    public class SEMAlgorithm: EMAlgorithm
    {
        private double[][] TempClusters { get; set; }
        private readonly int THRESHOLD_COEFFICIENT;

        public SEMAlgorithm(int amountOfClusters, IDistribution distribution, List<double> values, double eps) : base(
            amountOfClusters, distribution, values, eps)
        {
            THRESHOLD_COEFFICIENT = Convert.ToInt32(AmountOfElements * 0.05);
        }

        public override void SplitOnClusters()
        {
            Random random = new Random();
            FillProbabilityMatrixByRandomValues(random);
            var oldProbabilitiesMatrix = new double[AmountOfElements, AmountOfClusters];
            var index = 0;
            do
            {
                Array.Copy(Probabilities, oldProbabilitiesMatrix, AmountOfClusters * AmountOfElements);
                SStep(random);
                MStep();
                EStep();
                index++;
            } while ((CountChangesInProbabilitiesMatrix(oldProbabilitiesMatrix) > Eps) && (index < 500));

            if (IsClusterWithoutEnoughElements(THRESHOLD_COEFFICIENT))
            {
                AmountOfClusters--;
                SplitOnClusters();
            }
            else
            {
                SetUpLabels();
            }
        }

        private void UpdateProbabilityMatrix(Random random)
        {
            var oldProbabilities = Probabilities;
            Probabilities = new double[AmountOfElements, AmountOfClusters];
            for (int i = 0; i < AmountOfElements; i++)
            {
                var sortProbabilities = SortArrayWithProbabilities(oldProbabilities, i);
                var prob = random.NextDouble();
                int index = 0;
                double sum = sortProbabilities[index].Value;
                while (prob > sum)
                {
                    index++;
                    sum += sortProbabilities[index].Value;
                }

                Probabilities[i, sortProbabilities[index].Key] = 1;
            }
        }

        private KeyValuePair<int, double>[] SortArrayWithProbabilities(double[,] array, int index)
        {
            var dictionary = new Dictionary<int, double>();
            for (var i = 0; i < AmountOfClusters; i++)
            {
                dictionary.Add(i, array[index, i]);
            }

            return dictionary.OrderBy(p => p.Value).ToArray();
        }

        private void SStep(Random random)
        {
            UpdateProbabilityMatrix(random);
            SetUpLabels();
        }

        private bool IsClusterWithoutEnoughElements(int minAmountOfElementsInCluster)
        {
            for (var i = 0; i < AmountOfClusters; i++)
            {
                var amountOfElementsInCluster = Labels.Count(l => l == i);
                if (amountOfElementsInCluster < minAmountOfElementsInCluster)
                    return true;
            }

            return false;
        }
    }
}