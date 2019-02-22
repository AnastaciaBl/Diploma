using Diploma.Algorithms.Distribution;
using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            do
            {
                Array.Copy(Probabilities, oldProbabilitiesMatrix, AmountOfClusters * AmountOfElements);
                SStep(random);
                MStep();
                EStep();
            } while (CountChangesInProbabilitiesMatrix(oldProbabilitiesMatrix) > Eps);

            SetUpLabels();
        }

        private void UpdateProbabilityMatrix(Random random)
        {
            for (int i = 0; i < AmountOfElements; i++)
            {
                var newProbabilities = new List<double>();
                double checkSum = 0;
                for (int j = 0; j < AmountOfClusters; j++)
                {
                    var prob = random.NextDouble();
                    int index = 0;
                    double sum = Probabilities[i, index];
                    while (prob > sum)
                    {
                        index++;
                        sum += Probabilities[i, index];
                    }
                    newProbabilities.Add(Probabilities[i, index]);
                    checkSum += Probabilities[i, index];
                    //we need to check that sum of probabilities less than 1 and fix this if it is not
                    //TODO the same logic in 
                }

                if (checkSum > 1)
                {
                    newProbabilities.ForEach(p => p = p / checkSum);
                }
                else if (checkSum < 1)
                {
                    i--;
                }
            }
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
            TempClusters = new double[AmountOfClusters][];
            for (int i = 0; i < AmountOfClusters; i++)
            {
                var list = new List<double>();
                for (int j = 0; j < AmountOfElements; j++)
                {
                    if (Labels[j] == i)
                        list.Add(DataSetValues[j]);
                }

                TempClusters[i] = list.ToArray();
            }
        }

        protected override double[] CountAverage()
        {
            var averages = new double[AmountOfClusters];
            for (int i = 0; i < AmountOfClusters; i++)
            {
                averages[i] = TempClusters[i].Average();
            }
            return averages;
        }

        protected override double[] CountDispersion(double[] averages)
        {
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
            var probabilities = new double[AmountOfClusters];
            for (int i = 0; i < AmountOfClusters; i++)
                probabilities[i] = 1.0 * TempClusters[i].Length / AmountOfElements;
            return probabilities;
        }
    }
}
