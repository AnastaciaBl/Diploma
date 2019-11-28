using Aglomera;
using Aglomera.D3;
using Aglomera.Linkage;
using Diploma.Model;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Diploma.Algorithms.AgglomerativeHierarchic
{
    public class AgglomerativeHierarchicAglomera
    {
        private HashSet<DataPoint> DataSet { get; set; }
        private ClusteringResult<DataPoint> Result { get; set; }

        public AgglomerativeHierarchicAglomera(List<Patient> patients, double[][] data, int amountOfElements)
        {
            DataSet = CreateDataSet(patients, data, amountOfElements);
        }

        private HashSet<DataPoint> CreateDataSet(List<Patient> patients, double[][] data, int amountOfElements)
        {
            var set = new HashSet<DataPoint>();
            for (var i = 0; i < amountOfElements; i++)
            {
                set.Add(new DataPoint(patients[i].Id.ToString(), data[i]));
            }

            return set;
        }

        private void CreateJson()
        {
            Result.SaveD3DendrogramFile("result.json", formatting: Formatting.Indented);
        }

        public void SplitOnClusters()
        {
            var linkage = new AverageLinkage<DataPoint>(new DataPoint());
            var algorithm = new AgglomerativeClusteringAlgorithm<DataPoint>(linkage);
            Result = algorithm.GetClustering(DataSet);
            CreateJson();
        }
    }
}