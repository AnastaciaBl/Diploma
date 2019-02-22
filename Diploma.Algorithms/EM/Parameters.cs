namespace Diploma.Algorithms.EM
{
    public struct Parameters
    {
        public double MStruct; //average
        public double GStruct; //dispersion
        public double СStruct; //probability

        public Parameters(double m, double g, double c)
        {
            MStruct = m;
            GStruct = g;
            СStruct = c;
        }
    }
}
