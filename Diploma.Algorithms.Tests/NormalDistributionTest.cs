using Microsoft.VisualStudio.TestTools.UnitTesting;
using Diploma.Algorithms.Distribution;

namespace Diploma.Algorithms.Tests
{
    [TestClass]
    public class NormalDistributionTest
    {

        [DataTestMethod]
        [DataRow(0, 0.2, 0.0732)]
        [DataRow(3, 0.6, 0.0184)]
        [DataRow(3, 2, 0.1038)]
        public void NormalDistribution_Should_Return_Correct_Probability_Value(double m, double g, double etalon)
        {
            //Arrange
            var distribution = new NormalDistribution(m, g);

            //Act
            var answer = distribution.CountProbabilityFunctionResult(1);

            //Assert
            Assert.IsTrue(etalon == answer);
        }
    }
}
