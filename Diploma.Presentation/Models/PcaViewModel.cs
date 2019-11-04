using Diploma.Algorithms.PCA;

namespace Diploma.Presentation.Models
{
    public class PcaViewModel
    {
        public int Component { get; set; }
        public double EigenValue { get; set; }
        public double SingularValue { get; set; }
        public double Proportion { get; set; }
        public double CumulativeProportion { get; set; }
    }
}