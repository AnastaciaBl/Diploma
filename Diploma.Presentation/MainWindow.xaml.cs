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
            foreach (var p in patients)
            {
                data.Add(p.MemoryResult.SemanticMemoryTest.Result);
            }
            //var algor = new EMAlgorithm(4, new NormalDistribution(), data, 0.0001);
            var algor = new SEMAlgorithm(4, new NormalDistribution(), data, 0.0001);
            algor.SplitOnClusters();
            using (StreamWriter sw = new StreamWriter("answer.txt"))
            {
                for(int i=0;i<data.Count;i++)
                    sw.WriteLine(data[i] + "    " + algor.Labels[i]);
            }
            int a = 0;
        }
    }
}
