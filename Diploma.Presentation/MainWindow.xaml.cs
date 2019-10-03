using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Diploma.Data;
using Diploma.Model;
using Diploma.Algorithms.EM;
using Diploma.Algorithms.Distribution;
using System.IO;
using System.Windows.Controls.DataVisualization;
using System.Windows.Controls.DataVisualization.Charting;
using Diploma.Algorithms.Kmeans;
using Diploma.Algorithms.PCA;

namespace Diploma.Presentation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            var reader = new PatientReader();
            var patients = reader.ReadSetOfPatientsFromCsv("data.csv");
            SetDataToDataGrid(patients);

            //var data = new List<double>();

            //using (StreamReader sw = new StreamReader("data3.txt"))
            //{
            //    while (!sw.EndOfStream)
            //    {
            //        data.Add(Convert.ToDouble(sw.ReadLine()));
            //    }
            //}

            var patData1 = new List<double>();
            var patData2 = new List<double>();
            var patData3 = new List<double>();
            foreach (var p in patients)
            {
                //patData.Add(p.IntellectionResult.AnalogiesTest.Result);
                patData1.Add(p.EmotionalIntelligenceResult.PATest.Result);
                patData2.Add(p.EmotionalIntelligenceResult.UATest.Result);
                patData3.Add(p.EmotionalIntelligenceResult.VAETest.Result);
            }

            //var array = CreateDataSet(patData1, patData2);
            var array = CreateDataSet(patData1, patData2, patData3);

            //var algor = new EMAlgorithm(2, new NormalDistribution(), patData, 0.00001);
            //var algor = new SEMAlgorithm(2, new NormalDistribution(), patData, 0.00001);
            var algor = new KMeansAlgorithm(3, array);
            algor.SplitOnClusters();

            //var algor2 = new PCA(data);
            //var res = algor2.MakeComponents();

            //var algor = new KMeansAlgorithm(3, res);
            //algor.SplitOnClusters();



            //TwoScatter(patData1, patData2, algor.Labels);
            //ScatterChart(patData, algor.Labels);
            LineChart(patData1, patData2, patData3, algor.Labels);
        }

        public void ScatterChart(List<double> data, int[] labels)
        {
            var sChart1 = new PointCollection();
            var sChart2 = new PointCollection();
            var sChart3 = new PointCollection();

            for (int i = 0; i < data.Count; i++)
            {
                if (labels[i] == 0)
                    sChart1.Add(new Point(data[i], 0));
                else if (labels[i] == 1)
                    sChart2.Add(new Point(data[i], 0));
                else sChart3.Add(new Point(data[i], 0));
            }

            dataAfterEMChart.DataContext = new { points1 = sChart1, points2 = sChart2, points3 = sChart3 };
        }

        public void LineChart(List<double> x1, List<double> x2, List<double> x3, int[] labels)
        {
            var sChart1 = new PointCollection();
            var sChart2 = new PointCollection();
            var sChart3 = new PointCollection();

            for (int i = 0; i < labels.Length; i++)
            {
                if (labels[i] == 0)
                {
                    sChart1.Add(new Point(x1[i], 1));
                    sChart1.Add(new Point(x2[i], 1));
                    sChart1.Add(new Point(x3[i], 1));
                }
                else if (labels[i] == 1)
                {
                    sChart2.Add(new Point(x1[i], 2));
                    sChart2.Add(new Point(x2[i], 2));
                    sChart2.Add(new Point(x3[i], 2));
                }
                else
                {
                    sChart3.Add(new Point(x1[i], 3));
                    sChart3.Add(new Point(x2[i], 3));
                    sChart3.Add(new Point(x3[i], 3));
                }
            }

            DataLines.DataContext = new {points1 = sChart1, points2 = sChart2, points3 = sChart3};
        }

        public void TwoScatter(List<double> x, List<double> y, int[] labels)
        {
            var sChart1 = new PointCollection();
            var sChart2 = new PointCollection();
            var sChart3 = new PointCollection();

            for (int i = 0; i < labels.Length; i++)
            {
                if (labels[i] == 0)
                    sChart1.Add(new Point(x[i], y[i]));
                else if (labels[i] == 1)
                    sChart2.Add(new Point(x[i], y[i]));
                else sChart3.Add(new Point(x[i], y[i]));
            }

            TwoScatterChart.DataContext = new { points1 = sChart1, points2 = sChart2, points3 = sChart3 };
        }

        public double[][] CreateDataSet(List<double> x, List<double> y)
        {
            var array = new double[x.Count][];
            for (var i = 0; i < x.Count; i++)
            {
                array[i] = new [] { x[i], y[i] };
            }

            return array;
        }

        public double[][] CreateDataSet(List<double> x, List<double> x2, List<double> x3)
        {
            var array = new double[x.Count][];
            for (var i = 0; i < x.Count; i++)
            {
                array[i] = new[] { x[i], x2[i], x3[i] };
            }

            return array;
        }

        public void FillData(ref PointCollection array, List<double> data)
        {
            for (int i = 0; i < data.Count; i++)
            {
                array.Add(new Point(i, data[i]));
            }
        }

        public void SetDataToDataGrid(List<Patient> patients)
        {
            PatientsDataGrid.ItemsSource = patients;
        }
    }
}