using System;
using System.Linq;

namespace Diploma.Algorithms.StatisticalAnalysis
{
    public class BarChartData
    {
        public int AmountOfClasses { get; set; }
        public double[,] Borders { get; set; }
        public double[] Frequency { get; set; }
        public double Height { get; set; }

        public BarChartData(double[] elements)
        {
            AmountOfClasses = CountAmountOfClasses(elements.Length);
            Borders = FindClassesBorders(elements);
            Frequency = FindFrequency(elements);
        }

        private int CountAmountOfClasses(int amountOfElements)
        {
            var amount = (int)Math.Round(Math.Sqrt(amountOfElements));
            return amount % 2 == 0 ? amount - 1 : amount;
        }

        private double[,] FindClassesBorders(double[] elements)
        {
            double[,] borders = new double[AmountOfClasses, 2];
            var min = elements.Min();
            var max = elements.Max();
            Height = (max - min) / AmountOfClasses;
            for (int i = 1; i < AmountOfClasses + 1; i++)
            {
                borders[i - 1, 0] = Math.Round(min + (i - 1) * Height, 4);
                borders[i - 1, 1] = Math.Round(min + (i) * Height, 4);
            }
            return borders;
        }

        private double[] FindFrequency(double[] elements)
        {
            var frequency = new double[AmountOfClasses];
            var length = elements.Length;
            var sortElements = elements.OrderBy(x => x).ToArray();
            for (int i = 0; i < AmountOfClasses; i++)
            {
                int count = 0;
                for (int k = 0; k < length; k++)
                {
                    if (Borders[i, 0] <= sortElements[k] && Borders[i, 1] > sortElements[k])
                    {
                        count++;
                    }
                    else
                        if (i == AmountOfClasses - 1 && Borders[i, 1] == sortElements[k])
                    {
                        count++;
                    }
                }
                frequency[i] = (1.0 * count) / elements.Length;
            }
            return frequency;
        }
    }
}