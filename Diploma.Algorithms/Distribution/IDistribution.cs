namespace Diploma.Algorithms.Distribution
{
    interface IDistribution
    {
        //shift factor
        double M { get; set; }
        //scale factor
        double G { get; set; }
        double CountProbabilityFunctionResult(double xValue);
    }
}
