using System;
using System.Collections.Generic;
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
            EtalonHiddenVector.Add(new Parameters(-5, 0.2, 0.1));
            EtalonHiddenVector.Add(new Parameters(5, 0.2, 0.1));
            EtalonHiddenVector.Add(new Parameters(0, 0.1, 0.8));
            var sample = GenerateSample();
            
            //Act
            var emAlgorithm = new EMAlgorithm(AmountOfClusters, new NormalDistribution(), sample, Eps);
            emAlgorithm.SplitOnClusters();

            //Assert
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
            return sample;
        }
    }
}
