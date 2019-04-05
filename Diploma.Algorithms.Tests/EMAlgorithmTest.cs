using System;
using System.Collections.Generic;
using System.IO;
using Diploma.Algorithms.Distribution;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MathNet.Numerics;
using Diploma.Algorithms.EM;
using MathNet.Numerics.Distributions;

namespace Diploma.Algorithms.Tests
{
    [TestClass]
    public class EMAlgorithmTest
    {
        private List<Parameters> EtalonHiddenVector { get; set; }
        private int AmountOfElements { get; set; }
        private int AmountOfClusters { get; set; }
        private double Eps { get; set; }

        [TestInitialize]
        public void Setup()
        {
            EtalonHiddenVector = new List<Parameters>();
            Eps = 0.0001;
        }

        [TestMethod]
        public void TestEMAlgorithm()
        {
            //Arrange
            AmountOfElements = 1000;
            AmountOfClusters = 3;
            EtalonHiddenVector.Add(new Parameters(-5, 3, 0.2));
            EtalonHiddenVector.Add(new Parameters(5, 3, 0.2));
            EtalonHiddenVector.Add(new Parameters(0, 3, 0.6));
            //var sample = GenerateSample();

            List<double> sample = new List<double>();

            using (var sw = new StreamReader("data.txt"))
            {
                while (!sw.EndOfStream)
                {
                    sample.Add(Convert.ToDouble(sw.ReadLine()));
                }
            }

            /*var sample = new List<double>();
            sample.Add(-4.9);
            sample.Add(-5);
            sample.Add(-5.1);
            sample.Add(-0.1);
            sample.Add(0);
            sample.Add(0.1);
            sample.Add(4.9);
            sample.Add(5);
            sample.Add(5.1);*/

            //Act
            var emAlgorithm = new EMAlgorithm(AmountOfClusters, new NormalDistribution(), sample, Eps);
            emAlgorithm.SplitOnClusters();

            //var semAlgorithm = new SEMAlgorithm(AmountOfClusters, new NormalDistribution(), sample, Eps);
            //semAlgorithm.SplitOnClusters();

            //Assert
            //TODO finish the test
            Assert.IsTrue(false);
        }

        private List<double> GenerateSample()
        {
            var sample = new List<double>();
            for (int i = 0; i < AmountOfClusters; i++)
            {
                int amountOfElementsInCluster = (int) (EtalonHiddenVector[i].СStruct * AmountOfElements);
                Normal random = new Normal(EtalonHiddenVector[i].MStruct, Math.Sqrt(EtalonHiddenVector[i].GStruct));
                for (int j = 0; j < amountOfElementsInCluster; j++)
                {
                    sample.Add(random.Sample());
                }
            }

            using (var sw = new StreamWriter("data(3, 0.2-0.2-6).txt"))
            {
                foreach (var n in sample)
                {
                    sw.WriteLine(n);
                }
            }

            return sample;
        }
    }
}
