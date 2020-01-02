using Diploma.Algorithms.AgglomerativeHierarchic;
using Diploma.Algorithms.Criterion;
using Diploma.Algorithms.Distribution;
using Diploma.Algorithms.EM;
using Diploma.Algorithms.Kmeans;
using Diploma.Algorithms.PCA;
using Diploma.Algorithms.StatisticalAnalysis;
using Diploma.Data;
using Diploma.Model;
using Diploma.Presentation.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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
        private PCA PcaResult { get; set; }
        private AgglomerativeHierarchicAglomera Aglomera { get; set; }
        private List<int> ValuableIndexes { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            var legendStyle = new Style(typeof(Legend));
            legendStyle.Setters.Add(new Setter(VisibilityProperty, Visibility.Collapsed));
            legendStyle.Setters.Add(new Setter(WidthProperty, 0.0));
            legendStyle.Setters.Add(new Setter(HeightProperty, 0.0));
            AllPatientsChart.LegendStyle = legendStyle;
            OneAttributeBarChart.LegendStyle = legendStyle;
            AllPatientsAfterClusteringChart.LegendStyle = legendStyle;
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

            AttributeHierFirstCb.ItemsSource = attributes;
            AttributeHierFirstCb.SelectedIndex = 0;

            AttributeHierSecondCb.ItemsSource = attributes;
            AttributeHierSecondCb.SelectedIndex = 1;

            ComponentsPickOutCb.ItemsSource = new List<string> {"Eigen value < 1", "Sum of dispersion"};
            ComponentsPickOutCb.SelectedIndex = 0;

            ClusterAlgCb.ItemsSource = new List<string>() {"KMeans", "Hierarchic"};
            ClusterAlgCb.SelectedIndex = 0;

            AmountOfIndexesCb.ItemsSource = new List<string>() {"All indexes", "Valuable indexes"};
            AmountOfIndexesCb.SelectedIndex = 0;
        }

        public void FillAllPatientsChart()
        {
            var style = new Style(typeof(LineDataPoint));
            style.Setters.Add(new Setter(BackgroundProperty, Brushes.CornflowerBlue));
            style.Setters.Add(new Setter(Shape.StrokeThicknessProperty, 1.0));

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
            var maxAmountOfSteps = 100;
            var indexOfAttribute = AttributesCB.SelectedIndex;
            var data = GetDataOfAttribute(indexOfAttribute);
            int amountOfClusters = int.TryParse(AmountOfClustersTb.Text, out amountOfClusters) ? amountOfClusters : 2;
            int amountOfClasses = int.TryParse(AmountOfClassesTb.Text, out amountOfClasses) ? amountOfClasses : 0;
            EMAlgorithm algorithm = null;
            var distribution = new NormalDistribution();
            var eps = Constant.EPS;

            switch (AlgorithmCb.SelectedIndex)
            {
                case 0:
                    algorithm = new EMAlgorithm(distribution, data.ToList(), eps);
                    break;
                case 1:
                    algorithm = new SEMAlgorithm(distribution, data.ToList(), eps, true);
                    break;
            }
            algorithm.SplitOnClusters(amountOfClusters, maxAmountOfSteps);
            var labels = algorithm.Labels;
            HiddenVectorDG.ItemsSource = HiddenVectorViewModel.GetListOfHiddenVectorVM(algorithm.HiddenVector, algorithm.AmountOfClusters);
            FillBarChart(data, amountOfClasses);
            SetProbabilityDensityOnBarChart(data, algorithm.HiddenVector, algorithm.AmountOfClusters, labels);
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
            int amountOfClusters = int.TryParse(AmountOfClustersForStatisticTb.Text, out amountOfClusters) ? amountOfClusters : 1;
            int algIndex = ClusterAlgCb.SelectedIndex;
            int[] allAtrLabels = null;
            int[] valuableAttrLabels = null;

            AllPatientsAfterClusteringChart.Series.Clear();
            switch (algIndex)
            {
                case 0:
                    allAtrLabels = GetLabelsByKMeans(amountOfClusters, AttributeMatrix);
                    if (AmountOfIndexesCb.SelectedIndex == 1)
                    {
                        valuableAttrLabels = GetLabelsByKMeans(amountOfClusters,
                            GetAttributeMatrixByIndexes(ValuableIndexes));
                        FillChartAfterClustering(valuableAttrLabels, AllPatientsAfterClusteringChart);
                    }
                    break;
                case 1:
                    allAtrLabels = GetLabelsByHierarchic(amountOfClusters, AttributeMatrix);
                    if (AmountOfIndexesCb.SelectedIndex == 1)
                    {
                        valuableAttrLabels = GetLabelsByHierarchic(amountOfClusters,
                            GetAttributeMatrixByIndexes(ValuableIndexes));
                    }
                    break;
            }

            if (valuableAttrLabels != null)
            {
                RandIndexDg.ItemsSource = new List<RandIndexViewModel>()
                {
                    new RandIndexViewModel()
                    {
                        AllAttr = Constant.AMOUNT_OF_ATTRIBUTES,
                        AttrAfterReduction = ValuableIndexes.Count,
                        RandIndex = GetRandIndex(amountOfClusters, allAtrLabels, valuableAttrLabels)
                    }
                };

                FillChartAfterClustering(valuableAttrLabels, AllPatientsAfterClusteringChart);
            }
            else
            {
                RandIndexDg.ItemsSource = null;
                FillChartAfterClustering(allAtrLabels, AllPatientsAfterClusteringChart);
            }
        }

        private int[] GetLabelsByHierarchic(int amountOfClusters, double[][] dataSet)
        {
            var hierarchic = new AgglomerativeHierarchicAglomera(dataSet, AmountOfPatients);
            hierarchic.SplitOnClusters();
            return hierarchic.GetLabels(amountOfClusters);
        }

        private int[] GetLabelsByKMeans(int amountOfClusters, double[][] dataSet)
        {
            var kMeans = new KMeansAlgorithm();
            var labels = kMeans.SplitOnClusters(amountOfClusters, dataSet);
            return labels;
        }

        private double GetRandIndex(int amountOfClusters, int[] etalonLabels, int[] labels)
        {
            var index = new RandIndex();
            var matrix = index.CreateMatrixOfLabels(amountOfClusters, amountOfClusters, etalonLabels, labels);

            return index.GetRandIndex(matrix);
        }

        private double[][] GetAttributeMatrixByIndexes(List<int> indexes)
        {
            var matrix = new double[AmountOfPatients][];
            for (var i = 0; i < AmountOfPatients; i++)
            {
                matrix[i] = new double[indexes.Count];
                for (var j = 0; j < indexes.Count; j++)
                {
                    matrix[i][j] = AttributeMatrix[i][indexes[j]];
                }
            }

            return matrix;
        }

        public void FillChartAfterClustering(int[] labels, Chart chart)
        {
            for (var i = 0; i < AmountOfPatients; i++)
            {
                SolidColorBrush color = null;
                switch (labels[i])
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
                chart.Series.Add(series);
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
                LearnPca();
            }
        }

        private void LearnPca()
        {
            PcaResult = new PCA(AttributeMatrix);
            PcaResult.CreateComponents();

            var pcaViewModel = new List<PcaViewModel>();
            for (var i = 0; i < PcaResult.AmountOfComponents; i++)
                pcaViewModel.Add(new PcaViewModel()
                {
                    Component = i,
                    EigenValue = PcaResult.EigenValues[i],
                    SingularValue = PcaResult.SingularValues[i],
                    Proportion = PcaResult.Proportion[i],
                    CumulativeProportion = PcaResult.CumulativeProportion[i]
                });

            PcaParametersDg.ItemsSource = pcaViewModel;
        }

        private List<BICViewModel> CountBicCriteria(List<int> indexesOfMeaningfullParameters)
        {
            var bicList = new List<BICViewModel>();
            var amountOfClusters = 1;
            var paramAmount = 3;
            var maxAmountOfSteps = 30;
            var distribution = new NormalDistribution();
            for (var i = 0; i < indexesOfMeaningfullParameters.Count; i++)
            {
                var bicViewModel = new BICViewModel();
                bicViewModel.Component = indexesOfMeaningfullParameters[i];

                var em = new EMAlgorithm(distribution, PcaResult.GetVecorByIndex(indexesOfMeaningfullParameters[i]).ToList(), Constant.EPS);
                em.SplitOnClusters(amountOfClusters, maxAmountOfSteps);

                var bic = BIC.Count(distribution, paramAmount * amountOfClusters, em.DataSetValues, em.HiddenVector);
                bicViewModel.EM_BIC_1_cluster = Math.Round(bic, 3);

                var sem = new SEMAlgorithm(distribution, PcaResult.GetVecorByIndex(indexesOfMeaningfullParameters[i]).ToList(), Constant.EPS, false);
                sem.SplitOnClusters(amountOfClusters, maxAmountOfSteps);

                bic = BIC.Count(distribution, paramAmount * amountOfClusters, sem.DataSetValues, sem.HiddenVector);
                bicViewModel.SEM_BIC_1_cluster = Math.Round(bic, 3);

                amountOfClusters++;

                em.SplitOnClusters(amountOfClusters, maxAmountOfSteps);

                bic = BIC.Count(distribution, paramAmount * amountOfClusters, em.DataSetValues, em.HiddenVector);
                bicViewModel.EM_BIC_2_cluster = Math.Round(bic, 3);

                sem.SplitOnClusters(amountOfClusters, maxAmountOfSteps);

                bic = BIC.Count(distribution, paramAmount * amountOfClusters, sem.DataSetValues, sem.HiddenVector);
                bicViewModel.SEM_BIC_2_cluster = Math.Round(bic, 3);

                amountOfClusters = 1;

                if (bicViewModel.EM_BIC_1_cluster < bicViewModel.EM_BIC_2_cluster)
                    bicViewModel.EM_Recom_Amount_Of_Clusters = amountOfClusters;
                else bicViewModel.EM_Recom_Amount_Of_Clusters = amountOfClusters + 1;

                if (bicViewModel.SEM_BIC_1_cluster < bicViewModel.SEM_BIC_2_cluster)
                    bicViewModel.SEM_Recom_Amount_Of_Clusters = amountOfClusters;
                else bicViewModel.SEM_Recom_Amount_Of_Clusters = amountOfClusters + 1;

                bicList.Add(bicViewModel);
            }

            BicStatistic.ItemsSource = bicList;
            return bicList;
        }

        private void CountBicBtn_OnClick(object sender, RoutedEventArgs e)
        {
            List<int> indexList = null;
            switch (ComponentsPickOutCb.SelectedIndex)
            {
                case 0:
                    indexList = GetMeaningfullIndexesByEigenValueCriteria();
                    break;
                case 1:
                    double dispersionSum = double.TryParse(DispersionSumTb.Text, out dispersionSum) ? dispersionSum : 80;
                    indexList = GetMeaningfullIndexesBySumOfDispersionCriteria(dispersionSum);
                    break;
            }

            var bicList = CountBicCriteria(indexList);
            var autoSemList = CountAndPrintAutoSem(indexList);

            if (ExcludeComponentsRBtn.IsChecked == true)
            {
                for (var i = 0; i < indexList.Count; i++)
                {
                    if (bicList[i].EM_Recom_Amount_Of_Clusters == 1 && bicList[i].SEM_Recom_Amount_Of_Clusters == 1 &&
                        autoSemList[i].AutoAmountOfClusters == 1)
                    {
                        indexList.RemoveAt(i);
                        i--;
                    }
                }
            }

            var valuableParameterIndexes = PcaResult.GetValuableParameterIndexes(indexList, Patients.Count);
            ValuableIndexes = ValuableParametersViewModel.GetUniqueIndexes(valuableParameterIndexes, indexList.Count);
            var vIndexVm = new List<ValuableParametersViewModel>();
            foreach (var index in ValuableIndexes)
            {
                vIndexVm.Add(new ValuableParametersViewModel()
                {
                    Index = index,
                    Title = AttributesCB.Items[index].ToString()
                });
            }

            ValuableParametersDg.ItemsSource = vIndexVm;
        }

        private List<AutoSemViewModel> CountAndPrintAutoSem(List<int> indexesOfMeaningfullParameters)
        {
            var maxAmountOfSteps = 100;
            var distribution = new NormalDistribution();
            var amountOfClusters = 5;
            var results = new List<AutoSemViewModel>();
            for (var i = 0; i < indexesOfMeaningfullParameters.Count; i++)
            {
                var sem = new SEMAlgorithm(distribution, PcaResult.GetVecorByIndex(indexesOfMeaningfullParameters[i]).ToList(), Constant.EPS, true);
                sem.SplitOnClusters(amountOfClusters, maxAmountOfSteps);
                results.Add(new AutoSemViewModel()
                {
                    Component = indexesOfMeaningfullParameters[i],
                    AmountOfClusters = amountOfClusters,
                    ThresholdCoef = $"{Math.Round(sem.THRESHOLD_COEFFICIENT * 100.0 / sem.AmountOfElements)} %",
                    AutoAmountOfClusters = sem.AmountOfClusters
                }); 
            }

            BicSemAuto.ItemsSource = results;
            return results;
        }

        private List<int> GetMeaningfullIndexesByEigenValueCriteria(double eigenValue = 1)
        {
            var indexList = new List<int>();
            for (var i = 0; i < PcaResult.AmountOfComponents; i++)
            {
                if (PcaResult.EigenValues[i] > eigenValue)
                    indexList.Add(i);
            }

            return indexList;
        }

        private List<int> GetMeaningfullIndexesBySumOfDispersionCriteria(double dispersionSum)
        {
            var indexList = new List<int>();
            for (var i = 0; i < PcaResult.AmountOfComponents; i++)
            {
                if (PcaResult.CumulativeProportion[i] * 100 < dispersionSum)
                    indexList.Add(i);
                else
                {
                    break;
                }
            }

            return indexList;
        }

        private void ComponentsPickOutCb_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComponentsPickOutCb.SelectedIndex == 0)
            {
                DispersionSumTb.IsEnabled = false;
                DispersionSumTb.Text = string.Empty;
            }
            else
            {
                DispersionSumTb.IsEnabled = true;
                DispersionSumTb.Text = "80";
            }
        }

        private void HierarchicBtn_OnClick(object sender, RoutedEventArgs e)
        {
            Aglomera = new AgglomerativeHierarchicAglomera(AttributeMatrix, AmountOfPatients);
            Aglomera.SplitOnClusters();
            
            var silhouetteList = new List<HierarchicSilhouetteViewModel>();
            var daviesBouldinList = new List<HierarchicDaviesBouldinViewModel>();
            var calinskiList = new List<HierarchicCalinskiHarabaszViewModel>();
            for (var i = 0; i < Aglomera.SilhouetteCoefs.Count; i++)
            {
                silhouetteList.Add(new HierarchicSilhouetteViewModel()
                {
                    AmountOfClusters = i + 1,
                    SilhouetteIndex = Aglomera.SilhouetteCoefs[i]
                });

                daviesBouldinList.Add(new HierarchicDaviesBouldinViewModel()
                {
                    AmountOfClusters = i + 1,
                    DaviesBouldinIndex = Aglomera.DaviesBouldinCoefs[i]
                });

                calinskiList.Add(new HierarchicCalinskiHarabaszViewModel()
                {
                    AmountOfClusters = i + 1,
                    CalinskiHarabaszIndex = Aglomera.CalinskiHarabaszCoefs[i]
                });
            }
            SilhouetteIndexDg.ItemsSource = silhouetteList;
            DaviesBouldinIndexDg.ItemsSource = daviesBouldinList;
            CalinskiHarabaszIndexDg.ItemsSource = calinskiList;

            HierarchicBtn.IsEnabled = false;
            HierarchicVisualizeBtn.IsEnabled = true;
        }

        private void HierarchicVisualizeBtn_OnClick(object sender, RoutedEventArgs e)
        {
            VisualizeHierarchicChart();
        }

        private void VisualizeHierarchicChart()
        {
            HierarchicChart.Series.Clear();

            int amountOfClusters = int.TryParse(ClusterAmountHierarchicTb.Text, out amountOfClusters) ? amountOfClusters : 2;
            var labels = Aglomera.GetLabels(amountOfClusters);
            var firstAttributeIndex = AttributeHierFirstCb.SelectedIndex;
            var secondAttributeIndex = AttributeHierSecondCb.SelectedIndex;

            var dataF = GetDataOfAttribute(firstAttributeIndex);
            var dataS = GetDataOfAttribute(secondAttributeIndex);
            
            FillHierarchicChart(labels, dataF, dataS, amountOfClusters);
        }

        private void FillHierarchicChart(int[] labels, double[] firstAttributes, double[] secondAttributes, int amountOfClusters)
        {
            for (var i = 0; i < amountOfClusters; i++)
            {
                var points = new PointCollection();
                for (var j = 0; j < labels.Length; j++)
                {
                    if (labels[j] == i + 1)
                        points.Add(new Point(firstAttributes[j], secondAttributes[j]));
                }

                var series = new ScatterSeries()
                {
                    Title = $"{i + 1} cluster",
                    IndependentValuePath = "X",
                    DependentValuePath = "Y",
                    ItemsSource = points
                };

                HierarchicChart.Series.Add(series);
            }
        }

        private void ExcludeComponentsRBtn_OnClick(object sender, RoutedEventArgs e)
        {
            
        }
    }
}