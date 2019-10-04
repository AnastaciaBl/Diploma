using Diploma.Algorithms.Distribution;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Diploma.Algorithms.EM
{
    public class SEMAlgorithm: EMAlgorithm
    {
        private double[][] TempClusters { get; set; }

        public SEMAlgorithm(int amountOfClusters, IDistribution distribution, List<double> values, double eps) : base(
            amountOfClusters, distribution, values, eps)
        {
            
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

            SetUpLabels();
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

        protected override void EStep()
        {
            base.EStep();
        }

        protected override void MStep()
        {
            base.MStep();
        }

        protected override void SetUpLabels()
        {
            base.SetUpLabels();
            //TempClusters = new double[AmountOfClusters][];
            //for (int i = 0; i < AmountOfClusters; i++)
            //{
            //    var list = new List<double>();
            //    for (int j = 0; j < AmountOfElements; j++)
            //    {
            //        if (Labels[j] == i)
            //            list.Add(DataSetValues[j]);
            //    }

            //    TempClusters[i] = list.ToArray();
            //}
        }

        protected override double[] CountAverage()
        {
            return base.CountAverage();
            var averages = new double[AmountOfClusters];
            for (int i = 0; i < AmountOfClusters; i++)
            {
                averages[i] = TempClusters[i].Average();
            }
            return averages;
        }

        protected override double[] CountDispersion(double[] averages)
        {
            return base.CountDispersion(averages);
            var dispersions = new double[AmountOfClusters];
            for (int i = 0; i < AmountOfClusters; i++)
            {
                double sum = 0;
                TempClusters[i].ToList()
                    .ForEach(e => sum += Math.Pow(e - averages[i], 2));
                dispersions[i] = sum / TempClusters[i].Length;
            }
            return dispersions;
        }

        protected override double[] CountProbabilitiesToBeInCluster()
        {
            return base.CountProbabilitiesToBeInCluster();
            //var probabilities = new double[AmountOfClusters];
            //for (int i = 0; i < AmountOfClusters; i++)
            //    probabilities[i] = 1.0 * TempClusters[i].Length / AmountOfElements;
            //return probabilities;
        }
    }
}