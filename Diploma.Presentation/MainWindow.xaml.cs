using System;
using Diploma.Algorithms.Kmeans;
using Diploma.Data;
using Diploma.Presentation.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls.DataVisualization;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Media;
using System.Windows.Shapes;
using Diploma.Algorithms.Distribution;
using Diploma.Algorithms.EM;

namespace Diploma.Presentation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int AmountOfPatients { get; }
        private List<PatientViewModel> Patients { get; }
        private double[,] AttributeMatrix { get; }

        public MainWindow()
        {
            InitializeComponent();

            var reader = new PatientReader();
            var patients = reader.ReadSetOfPatientsFromCsv("data.csv");
            AmountOfPatients = patients.Count;
            AttributeMatrix = reader.GetAttributesMatrix(patients);
            Patients = PatientViewModel.SetListOfPatientViewModel(patients);
            FillDataToElements();
            //FillAllPatientsChart();

            
            //var algor = new EMAlgorithm(2, new NormalDistribution(), patData, 0.00001);
            //var algor = new SEMAlgorithm(2, new NormalDistribution(), patData, 0.00001);
            //var algor = new KMeansAlgorithm(3, array);
            //algor.SplitOnClusters();
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

        public void FillDataToElements()
        {
            PatientsDataGrid.ItemsSource = Patients;

            var attributes = new List<string>
            {
                "FirstTest",
                "SecondTest",
                "ThirdTest",
                "FourthTest",
                "FifthTest",
                "AfterFourtyMinutesTest",
                "FigurativeMemoryTest",
                "Time",
                "Result",
                "SemanticMemoryTest",
                "VMemoryTest",
                "MunstAttentionTest",
                "EUTest",
                "CAVTest",
                "ARTest",
                "VRTest",
                "PUTest",
                "MPTest",
                "MUTest",
                "VPTest",
                "VUTest",
                "VATest",
                "MAETest",
                "VAETest",
                "PATest",
                "UATest",
                "OUTest",
                "COTTest",
                "SPTest",
                "EbbinghouseTest",
                "EliminateUnnecessaryTest",
                "AnalogiesTest"
            };
            AttributesCB.ItemsSource = attributes;
            AttributesCB.SelectedIndex = 0;

            AlgorithmCB.ItemsSource = new List<string>{ "EM", "SEM"};
            AlgorithmCB.SelectedIndex = 0;
        }

        public void FillAllPatientsChart()
        {
            var style = new Style(typeof(DataPoint));
            style.Setters.Add(new Setter(Shape.StrokeProperty, Brushes.Blue));
            style.Setters.Add(new Setter(Shape.StrokeThicknessProperty, 1.0));

            var legendStyle = new Style(typeof(Legend));
            legendStyle.Setters.Add(new Setter(VisibilityProperty, Visibility.Collapsed));
            legendStyle.Setters.Add(new Setter(WidthProperty, 0.0));
            legendStyle.Setters.Add(new Setter(HeightProperty, 0.0));
            AllPatientsChart.LegendStyle = legendStyle;

            foreach (var p in Patients)
            {
                var series = new LineSeries
                {
                    DataPointStyle = style,
                    IndependentValuePath = "X",
                    DependentValuePath = "Y"
                };
                var values = PatientViewModel.GetPatientAttributes(p);

                var points = new PointCollection();
                for (var i = 0; i < values.Length; i++)
                    points.Add(new Point(i, values[i]));

                series.ItemsSource = points;
                AllPatientsChart.Series.Add(series);
            }
        }

        private void SplitOneAttribute_OnClick(object sender, RoutedEventArgs e)
        {
            var indexOfAttribute = AttributesCB.SelectedIndex;
            var data = GetDataOfAttribute(indexOfAttribute);
            int amountOfClusters = int.TryParse(AmountOfClusterTB.Text, out amountOfClusters) ? amountOfClusters : 2;
            EMAlgorithm algorithm = null;
            var distribution = new NormalDistribution();
            var eps = 0.00001;

            switch (AlgorithmCB.SelectedIndex)
            {
                case 0:
                    algorithm = new EMAlgorithm(amountOfClusters, distribution, data.ToList(), eps);
                    break;
                case 1:
                    algorithm = new SEMAlgorithm(amountOfClusters, distribution, data.ToList(), eps);
                    break;
            }
            algorithm.SplitOnClusters();
            var labels = algorithm.Labels;
            HiddenVectorDG.ItemsSource = HiddenVectorViewModel.GetListOfHiddenVectorVM(algorithm.HiddenVector, amountOfClusters);
        }

        private double[] GetDataOfAttribute(int index)
        {
            var array = new double[AmountOfPatients];
            for (var i = 0; i < AmountOfPatients; i++)
            {
                array[i] = AttributeMatrix[i, index];
            }

            return array;
        }
    }
}