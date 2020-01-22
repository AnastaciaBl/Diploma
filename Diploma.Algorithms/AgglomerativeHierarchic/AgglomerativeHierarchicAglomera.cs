using Aglomera;
using Aglomera.D3;
using Aglomera.Evaluation.Internal;
using Aglomera.Linkage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Diploma.Algorithms.AgglomerativeHierarchic
{
    public class AgglomerativeHierarchicAglomera
    {
        private HashSet<DataPoint> DataSet { get; set; }
        private ClusteringResult<DataPoint> Result { get; set; }
        public List<double> SilhouetteCoefs { get; set; }
        public List<double> CalinskiHarabaszCoefs { get; set; }
        public List<double> DaviesBouldinCoefs { get; set; }

        public AgglomerativeHierarchicAglomera(double[][] data, int amountOfElements)
        {
            DataSet = CreateDataSet(data, amountOfElements);
            SilhouetteCoefs = new List<double>();
            CalinskiHarabaszCoefs = new List<double>();
            DaviesBouldinCoefs = new List<double>();
        }

        private HashSet<DataPoint> CreateDataSet(double[][] data, int amountOfElements)
        {
            var set = new HashSet<DataPoint>();
            for (var i = 0; i < amountOfElements; i++)
            {
                set.Add(new DataPoint(i.ToString(), data[i]));
            }

            return set;
        }

        private void CreateJson()
        {
            Result.SaveD3DendrogramFile("result.json", formatting: Formatting.Indented);
        }

        public void SplitOnClusters(int algoritmIndex)
        {
            AgglomerativeClusteringAlgorithm<DataPoint> algorithm = null;
            switch (algoritmIndex)
            {
                case 1:
                    var linkage1 = new CompleteLinkage<DataPoint>(new DataPoint());
                    algorithm = new AgglomerativeClusteringAlgorithm<DataPoint>(linkage1);
                    break;
                case 2:
                    var linkage2 = new AverageLinkage<DataPoint>(new DataPoint());
                    algorithm = new AgglomerativeClusteringAlgorithm<DataPoint>(linkage2);
                    break;
                case 3:
                    var linkage3 = new CentroidLinkage<DataPoint>(new DataPoint(), DataPoint.GetCentroid);
                    algorithm = new AgglomerativeClusteringAlgorithm<DataPoint>(linkage3);
                    break;
                case 4:
                    var linkage4 = new MinimumEnergyLinkage<DataPoint>(new DataPoint());
                    algorithm = new AgglomerativeClusteringAlgorithm<DataPoint>(linkage4);
                    break;
                case 5:
                    var linkage5 = new SingleLinkage<DataPoint>(new DataPoint());
                    algorithm = new AgglomerativeClusteringAlgorithm<DataPoint>(linkage5);
                    break;
                case 6:
                    var linkage6 = new WardsMinimumVarianceLinkage<DataPoint>(new DataPoint(), DataPoint.GetCentroid);
                    algorithm = new AgglomerativeClusteringAlgorithm<DataPoint>(linkage6);
                    break;
            }
            
            Result = algorithm.GetClustering(DataSet);
            CreateJson();

            SilhouetteCoefs = GetSilhouetteCoefficients();
            CalinskiHarabaszCoefs = GetCalinskiHarabaszCoefficients();
            DaviesBouldinCoefs = GetDaviesBouldinCoefficients();
        }

        public List<double> GetSilhouetteCoefficients()
        {
            var coefs = new List<double>();
            var criterion = new SilhouetteCoefficient<DataPoint>(new DataPoint());
            for (var i = Result.Count - 2; i >= 1; i--)
            {
                coefs.Add(criterion.Evaluate(Result[i]));
            }
            return coefs;
        }

        public List<double> GetCalinskiHarabaszCoefficients()
        {
            var coefs = new List<double>();
            var criterion = new SilhouetteCoefficient<DataPoint>(new DataPoint());
            for (var i = Result.Count - 2; i >= 1; i--)
            {
                coefs.Add(criterion.Evaluate(Result[i]));
            }
            return coefs;
        }

        public List<double> GetDaviesBouldinCoefficients()
        {
            var coefs = new List<double>();
            var criterion = new SilhouetteCoefficient<DataPoint>(new DataPoint());
            for (var i = Result.Count - 2; i >= 1; i--)
            {
                coefs.Add(criterion.Evaluate(Result[i]));
            }
            return coefs;
        }

        public int[] GetLabels(int amountOfClusters)
        {
            var result = Result[Result.Count - amountOfClusters];
            var labels = new int[DataSet.Count];

            var clusterIndex = 1;
            foreach (var clusterSet in result)
            {
                foreach (var cluster in clusterSet)
                {
                    labels[Convert.ToInt32(cluster.ID)] = clusterIndex;
                }

                clusterIndex++;
            }

            return labels;
        }
    }
}