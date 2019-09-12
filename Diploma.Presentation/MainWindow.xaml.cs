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
using System.Windows.Controls.DataVisualization.Charting;

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
            var data = new List<double>();

            using (StreamReader sw = new StreamReader("data3.txt"))
            {
                while (!sw.EndOfStream)
                {
                    data.Add(Convert.ToDouble(sw.ReadLine()));
                }
            }

            var fChart = new PointCollection();
            var sChart1 = new PointCollection();
            var sChart2 = new PointCollection();
            var sChart3 = new PointCollection();

            //var algor = new EMAlgorithm(3, new NormalDistribution(), data, 0.00001);
            var algor = new SEMAlgorithm(3, new NormalDistribution(), data, 0.00001);
            algor.SplitOnClusters();

            for (int i = 0; i < data.Count; i++)
            {
                fChart.Add(new Point(i, data[i]));
                if(algor.Labels[i] == 0)
                    sChart1.Add(new Point(i, data[i]));
                else if (algor.Labels[i] == 1)
                    sChart2.Add(new Point(i, data[i]));
                else sChart3.Add(new Point(i, data[i]));
            }

            dataChart.DataContext = new {points = fChart};
            dataAfterEMChart.DataContext = new {points1 = sChart1, points2 = sChart2, points3 = sChart3};
        }

        public void FillData(ref PointCollection array, List<double> data)
        {
            for (int i = 0; i < data.Count; i++)
            {
                array.Add(new Point(i, data[i]));
            }
        }
    }
}