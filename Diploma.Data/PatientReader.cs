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
            using (var sr = new StreamReader(filePath))
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
            patient.Name = splitData[2];
            patient.FinalResult = PickOutTheFinalResult(splitData[4]);
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
    }
}
