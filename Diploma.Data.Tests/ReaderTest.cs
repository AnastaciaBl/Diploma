using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Diploma.Data.Tests
{
    [TestClass]
    public class ReaderTest
    {
        private readonly string filePath = @"C:\Users\ablyz\Desktop\Diploma\Program\data.csv";
        private Reader reader;

        [TestInitialize]
        public void Setup()
        {
            reader = new Reader();
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
            Assert.AreSame(patients.Count, amountOfPatients);
        }


        [DataTestMethod]
        [DataRow(8, "Соловьев", 13)]
        [DataRow(33, "Чабан", 14)]
        [DataRow(112, "Чубарь Ю.Н.", 13)]
        [DataRow(1, "Коваль", 11)]
        public void List_Has_Correct_Data(int id, string name, double munstAttentionTestResult)
        {
            //Act
            var patients = reader.ReadSetOfPatientsFromCsv(filePath);
            var controlPatient = patients.Where(p => p.Id == id).First();

            //Assert
            Assert.AreSame(controlPatient.Name, name);
            Assert.AreSame(controlPatient.AttentionResult.MunstAttentionTest, munstAttentionTestResult);
        }
    }
}
