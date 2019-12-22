using System.Collections.Generic;
using Diploma.Model;

namespace Diploma.Presentation.Models
{
    public class ValuableParametersViewModel
    {
        public int Index { get; set; }
        public string Title { get; set; }

        public static List<int> GetUniqueIndexes(int[][] allIndexes, int amountOfParameters)
        {
            var indexList = new List<int>();
            for (var i = 0; i < amountOfParameters; i++)
            {
                for (var j = 0; j < allIndexes[i].Length; j++)
                {
                    if(!indexList.Contains(allIndexes[i][j]))
                        indexList.Add(allIndexes[i][j]);
                }
            }

            return indexList;
        }
    }
}