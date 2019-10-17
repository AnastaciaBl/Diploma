using Diploma.Algorithms.Distribution;
using Diploma.Algorithms.EM;
using Diploma.Algorithms.Kmeans;
using Diploma.Algorithms.StatisticalAnalysis;
using Diploma.Data;
using Diploma.Model;
using Diploma.Presentation.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls.DataVisualization;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Diploma.Presentation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int AmountOfPatients { get; set; }
        private List<Patient> Patients { get; set; }
        private List<PatientViewModel> PatientsVM { get; set; }
        private double[][] AttributeMatrix { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        //can be useful
        public void FillData(ref PointCollection array, List<double> data)
        {
            for (int i = 0; i < data.Count; i++)
            {
                array.Add(new Point(i, data[i]));
            }
        }

        public void FillDataToElements()
        {
            PatientsDataGrid.ItemsSource = PatientsVM;

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

            AlgorithmCb.ItemsSource = new List<string>{ "EM", "SEM"};
            AlgorithmCb.SelectedIndex = 0;
        }

        public void FillAllPatientsChart()
        {
            var style = new Style(typeof(LineDataPoint));
            style.Setters.Add(new Setter(BackgroundProperty, Brushes.CornflowerBlue));
            style.Setters.Add(new Setter(Shape.StrokeThicknessProperty, 1.0));

            var legendStyle = new Style(typeof(Legend));
            legendStyle.Setters.Add(new Setter(VisibilityProperty, Visibility.Collapsed));
            legendStyle.Setters.Add(new Setter(WidthProperty, 0.0));
            legendStyle.Setters.Add(new Setter(HeightProperty, 0.0));
            AllPatientsChart.LegendStyle = legendStyle;

            foreach (var p in PatientsVM)
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
            int amountOfClusters = int.TryParse(AmountOfClustersTb.Text, out amountOfClusters) ? amountOfClusters : 2;
            int amountOfClasses = int.TryParse(AmountOfClassesTb.Text, out amountOfClasses) ? amountOfClasses : 0;
            EMAlgorithm algorithm = null;
            var distribution = new NormalDistribution();
            var eps = 0.00001;

            switch (AlgorithmCb.SelectedIndex)
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
            FillBarChart(data, amountOfClasses);
            SetProbabilityDensityOnBarChart(data, algorithm.HiddenVector, amountOfClusters, labels);
        }

        private double[] GetDataOfAttribute(int index)
        {
            var array = new double[AmountOfPatients];
            for (var i = 0; i < AmountOfPatients; i++)
            {
                array[i] = AttributeMatrix[i][index];
            }

            return array;
        }

        private void SetProbabilityDensityOnBarChart(double[] data, List<Parameters> hiddenVector, int amountOfClusters, int[] labels)
        {
            var distribution = new NormalDistribution();
            var x = data.Min();
            var amountOfPoints = (int)(data.Max() - x) * 10;
            var smoothLinePoints = new PointCollection();
            var smoothLineSeries = new LineSeries()
            {
                Title = "Probability density",
                IndependentValuePath = "X",
                DependentValuePath = "Y"
            };
            for (var i = 0; i < amountOfClusters; i++)
            {
                var scatterSeries = new ScatterSeries()
                {
                    Title = $"{i + 1} cluster",
                    IndependentValuePath = "X",
                    DependentValuePath = "Y"
                };
                var scatterPoints = new PointCollection();

                for (var j = 0; j < data.Length; j++)
                {
                    if (labels[j] == i)
                        scatterPoints.Add(new Point(data[j], 0));
                }

                for (var j = 0; j < amountOfPoints; j++)
                {
                    if (i + 1 == amountOfClusters)
                    {
                        double p = 0;
                        for (var t = 0; t < amountOfClusters; t++)
                        {
                            p += hiddenVector[t].СStruct * distribution.CountProbabilityFunctionResult(
                                     hiddenVector[t].MStruct,
                                     hiddenVector[t].GStruct, x);
                        }
                        smoothLinePoints.Add(new Point(x, p));
                        x += 0.1;
                    }
                }
                scatterSeries.ItemsSource = scatterPoints;
                OneAttributeBarChart.Series.Add(scatterSeries);
            }
            smoothLineSeries.ItemsSource = smoothLinePoints;
            OneAttributeBarChart.Series.Add(smoothLineSeries);
        }

        private void AmountOfClusterKMeans_OnClick(object sender, RoutedEventArgs e)
        {
            var amountOfClusters = Convert.ToInt32(AmountOfClustersKMeansTB.Text);
            var kMeans = new KMeansAlgorithm(amountOfClusters, AttributeMatrix);
            kMeans.SplitOnClusters();

            AllPatientsChart.Series.Clear();
            for (var i = 0; i < AmountOfPatients; i++)
            {
                SolidColorBrush color = null;
                switch (kMeans.Labels[i])
                {
                    case 0:
                        color = new SolidColorBrush(Colors.Blue);
                        break;
                    case 1:
                        color = new SolidColorBrush(Colors.Crimson);
                        break;
                    case 2:
                        color = new SolidColorBrush(Colors.DarkMagenta);
                        break;
                    case 3:
                        color = new SolidColorBrush(Colors.DarkOrange);
                        break;
                    case 4:
                        color = new SolidColorBrush(Colors.DarkGreen);
                        break;
                    case 5:
                        color = new SolidColorBrush(Colors.HotPink);
                        break;
                    case 6:
                        color = new SolidColorBrush(Colors.DeepSkyBlue);
                        break;
                    case 7:
                        color = new SolidColorBrush(Colors.Chartreuse);
                        break;
                    default:
                        color = new SolidColorBrush(Colors.Gold);
                        break;
                }

                var style = new Style(typeof(LineDataPoint));
                style.Setters.Add(new Setter(BackgroundProperty, color));

                var series = new LineSeries()
                {
                    IndependentValuePath = "X",
                    DependentValuePath = "Y",
                    DataPointStyle = style
                };
                var points = new PointCollection();

                for (var j = 0; j < Constant.AMOUNT_OF_ATTRIBUTES; j++)
                {
                    points.Add(new Point(j, AttributeMatrix[i][j]));
                }

                series.ItemsSource = points;
                AllPatientsChart.Series.Add(series);
            }
        }
        
        private void SplitOnClasses_OnClick(object sender, RoutedEventArgs e)
        {
            OneAttributeBarChart.Series.Clear();

            var legendStyle = new Style(typeof(Legend));
            legendStyle.Setters.Add(new Setter(WidthProperty, 0.0));
            legendStyle.Setters.Add(new Setter(HeightProperty, 0.0));
            OneAttributeBarChart.LegendStyle = legendStyle;

            var indexOfAttribute = AttributesCB.SelectedIndex;
            int amountOfClasses = int.TryParse(AmountOfClassesTb.Text, out amountOfClasses) ? amountOfClasses : 0;
            var data = GetDataOfAttribute(indexOfAttribute);

            FillBarChart(data, amountOfClasses);
        }

        private void FillBarChart(double[] data, int amountOfClasses)
        {
            var barChartData = new BarChartData(data, amountOfClasses);

            var style = new Style(typeof(AreaDataPoint));
            style.Setters.Add(new Setter(BackgroundProperty, Brushes.Coral));

            for (var i = 0; i < barChartData.AmountOfClasses; i++)
            {
                var frequency = Math.Round(barChartData.Frequency[i] / barChartData.Height, 3);
                var values = new PointCollection();
                var areaSeries = new AreaSeries()
                {
                    IndependentValuePath = "X",
                    DependentValuePath = "Y",
                    DataPointStyle = style
                };
                values.Add(new Point(barChartData.Borders[i, 0], frequency));
                values.Add(new Point(barChartData.Borders[i, 1], frequency));
                areaSeries.ItemsSource = values;
                OneAttributeBarChart.Series.Add(areaSeries);
            }
        }

        private void OpenFileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                var filePath = openFileDialog.FileName;
                var reader = new PatientReader();
                Patients = reader.ReadSetOfPatientsFromCsv(filePath);
                AmountOfPatients = Patients.Count;
                AttributeMatrix = reader.GetAttributesMatrix(Patients);
                PatientsVM = PatientViewModel.SetListOfPatientViewModel(Patients);
                FillDataToElements();
                FillAllPatientsChart();
            }
        }
    }
}