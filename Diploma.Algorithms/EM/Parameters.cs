namespace Diploma.Algorithms.EM
{
    public struct Parameters
    {
        public double MStruct;
        public double GStruct;
        public double СStruct;

        public Parameters(double m, double g, double c)
        {
            MStruct = m;
            GStruct = g;
            СStruct = c;
        }
    }
}
