using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Diploma.Data.Tests
{
    [TestClass]
    public class ReaderTest
    {
        private readonly string filePath = @"C:\Users\ablyz\Desktop\Diploma\Program\DataSet\data.csv";
        private PatientReader reader;

        [TestInitialize]
        public void Setup()
        {
            reader = new PatientReader();
        }

        [TestMethod]
        public void List_InNot_Null()
        {
            //Act
            var patients = reader.ReadSetOfPatientsFromCsv(filePath);

            //Assert
            Assert.IsNotNull(patients);
        }

        [TestMethod]
        public void List_Has_Correct_Amount_Of_Elements()
        {
            //Arrange
            int amountOfPatients = 84;

            //Act
            var patients = reader.ReadSetOfPatientsFromCsv(filePath);

            //Assert
            Assert.IsTrue(patients.Count == amountOfPatients);
        }


        [DataTestMethod]
        [DataRow(8, "Соловьев", 13)]
        [DataRow(33, "Чабан", 14)]
        [DataRow(112, "Чубарь Ю.Н.", 13)]
        [DataRow(1, "Коваль", 11)]
        public void List_Has_Correct_Data_MunstAttentionTest(int id, string name, double munstAttentionTestResult)
        {
            //Act
            var patients = reader.ReadSetOfPatientsFromCsv(filePath);
            var controlPatient = patients.Where(p => p.Id == id).First();

            //Assert
            Assert.IsTrue(controlPatient.Name == name);
            Assert.IsTrue(controlPatient.AttentionResult.MunstAttentionTest.Result == munstAttentionTestResult);
        }

        [DataTestMethod]
        [DataRow(8, "Соловьев", 4)]
        [DataRow(33, "Чабан", 8)]
        [DataRow(112, "Чубарь Ю.Н.", 9)]
        [DataRow(1, "Коваль", 7)]
        public void List_Has_Correct_Data_EbbinghouseTest(int id, string name, double ebbinghouseTestResult)
        {
            //Act
            var patients = reader.ReadSetOfPatientsFromCsv(filePath);
            var controlPatient = patients.Where(p => p.Id == id).First();

            //Assert
            Assert.IsTrue(controlPatient.Name == name);
            Assert.IsTrue(controlPatient.IntellectionResult.EbbinghouseTest.Result == ebbinghouseTestResult);
        }

        [DataTestMethod]
        [DataRow(94, "Шевчук В.К.", -1)]
        public void Correct_Handle_Empty_Data(int id, string name, double semanticMemoryTestResult)
        {
            //Act
            var patients = reader.ReadSetOfPatientsFromCsv(filePath);
            var controlPatient = patients.Where(p => p.Id == id).First();

            //Assert
            Assert.IsTrue(controlPatient.Name == name);
            Assert.IsTrue(controlPatient.MemoryResult.SemanticMemoryTest.Result == semanticMemoryTestResult);
        }
    }
}
