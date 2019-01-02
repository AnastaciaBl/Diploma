namespace Diploma.Model
{
    public class TestResult
    {
        public double Result { get; set; }
        public double MinNorm { get; set; }
        public double MaxNorm { get; set; }

        public bool IsNorm
        {
            get
            {
                if (Result >= MinNorm && Result <= MaxNorm)
                    return true;
                else
                    return false;
            }
        }
}
}
