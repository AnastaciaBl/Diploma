using Diploma.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Data
{
    public class PatientReader
    {
        public List<Patient> ReadSetOfPatientsFromCsv(string filePath)
        {
            var patients = new List<Patient>();
            var dataFromFile = ReadLinesFromCsv(filePath);
            foreach (var str in dataFromFile)
            {
                patients.Add(CreatePatient(str));
            }
            return patients;
        }

        private List<string> ReadLinesFromCsv(string filePath)
        {
            var data = new List<string>();
            using (var sr = new StreamReader(filePath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    data.Add(sr.ReadLine());
                }
            }
            return data;
        }

        private Patient CreatePatient(string data)
        {
            var splitData = data.Split(';');
            var patient = new Patient();
            patient.Id = Int32.Parse(splitData[0]);
            patient.RegistrationYear = PickOutTheRegistrationYear(splitData[1]);
            //splitData[2] - useless data
            patient.Name = splitData[3];
            patient.FinalResult = PickOutTheFinalResult(splitData[4]);
            #region SetTestsResults
            //memory tests
            patient.MemoryResult.TenWordsTest.FirstTest.Result = ParseStringToDouble(splitData[5]);
            patient.MemoryResult.TenWordsTest.SecondTest.Result = ParseStringToDouble(splitData[6]);
            patient.MemoryResult.TenWordsTest.ThirdTest.Result = ParseStringToDouble(splitData[7]);
            patient.MemoryResult.TenWordsTest.FourthTest.Result = ParseStringToDouble(splitData[8]);
            patient.MemoryResult.TenWordsTest.FifthTest.Result = ParseStringToDouble(splitData[9]);
            patient.MemoryResult.TenWordsTest.AfterFourtyMinutesTest.Result = ParseStringToDouble(splitData[10]);
            
            patient.MemoryResult.FigurativeMemoryTest.Result = ParseStringToDouble(splitData[11]);

            patient.MemoryResult.VisualMemoryTest.Result = ParseStringToDouble(splitData[12]);
            patient.MemoryResult.VisualMemoryTest.Time = Int32.Parse(splitData[13]);

            patient.MemoryResult.SemanticMemoryTest.Result = ParseStringToDouble(splitData[14]);

            patient.MemoryResult.VMemoryTest.Result = ParseStringToDouble(splitData[15]);

            //attention tests
            patient.AttentionResult.MunstAttentionTest.Result = ParseStringToDouble(splitData[16]);

            patient.AttentionResult.CorTableAttentionTest.EUTest.Result = ParseStringToDouble(splitData[17]);
            patient.AttentionResult.CorTableAttentionTest.CAVTest.Result = ParseStringToDouble(splitData[18]);

            patient.AttentionResult.SchulteTableAttentionTest.ARTest.Result = ParseStringToDouble(splitData[19]);
            patient.AttentionResult.SchulteTableAttentionTest.VRTest.Result = ParseStringToDouble(splitData[20]);
            patient.AttentionResult.SchulteTableAttentionTest.PUTest.Result = ParseStringToDouble(splitData[21]);

            //emotional intelligence tests
            patient.EmotionalIntelligenceResult.MPTest.Result = ParseStringToDouble(splitData[22]);
            patient.EmotionalIntelligenceResult.MUTest.Result = ParseStringToDouble(splitData[23]);
            patient.EmotionalIntelligenceResult.VPTest.Result = ParseStringToDouble(splitData[24]);
            patient.EmotionalIntelligenceResult.VUTest.Result = ParseStringToDouble(splitData[25]);
            patient.EmotionalIntelligenceResult.VATest.Result = ParseStringToDouble(splitData[26]);
            patient.EmotionalIntelligenceResult.MAETest.Result = ParseStringToDouble(splitData[27]);
            patient.EmotionalIntelligenceResult.VAETest.Result = ParseStringToDouble(splitData[28]);
            patient.EmotionalIntelligenceResult.PATest.Result = ParseStringToDouble(splitData[29]);
            patient.EmotionalIntelligenceResult.UATest.Result = ParseStringToDouble(splitData[30]);
            patient.EmotionalIntelligenceResult.OUTest.Result = ParseStringToDouble(splitData[31]);

            //intellection tests
            patient.IntellectionResult.COTTest.Result = ParseStringToDouble(splitData[32]);
            patient.IntellectionResult.SPTest.Result = ParseStringToDouble(splitData[33]);
            patient.IntellectionResult.EbbinghouseTest.Result = ParseStringToDouble(splitData[34]);
            patient.IntellectionResult.EliminateUnnecessaryTest.Result = ParseStringToDouble(splitData[35]);
            patient.IntellectionResult.AnalogiesTest.Result = ParseStringToDouble(splitData[36]);

            #endregion
            return patient;
        }

        private int PickOutTheRegistrationYear(string line)
        {
            //"2014 г."
            var year = line.Split(' ');
            return Int32.Parse(year[0]);
        }

        private double PickOutTheFinalResult(string line)
        {
            //"I 69.3"
            var result = line.Split(' ');
            return Double.Parse(result[1].Replace(".", ","));
        }

        private double ParseStringToDouble(string value)
        {
            try
            {
                if(value == "NaN")
                    throw new FormatException();
                return Double.Parse(value);
            }
            catch (FormatException e)
            {
                return -1;
            }
        }
    }
}
