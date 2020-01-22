using System;
using Diploma.Model;
using System.Collections.Generic;

namespace Diploma.Presentation.Models
{
    public class PatientViewModel
    {
        public int Id { get; set; }
        //public int RegistrationYear { get; set; }
        //public double FinalResult { get; set; }

        //*****Memory

        //10 words
        public double FirstTest { get; set; }
        public double SecondTest { get; set; }
        public double ThirdTest { get; set; }
        public double FourthTest { get; set; }
        public double FifthTest { get; set; }
        public double AfterFourtyMinutesTest { get; set; }

        //
        public double FigurativeMemoryTest { get; set; }

        //visual
        public int Time { get; set; }
        public double Result { get; set; }

        //
        public double SemanticMemoryTest { get; set; }

        //
        public double VMemoryTest { get; set; }

        //******Attention

        //
        public double MunstAttentionTest { get; set; }

        //CorTableAttentionTest
        public double EUTest { get; set; }
        public double CAVTest { get; set; }

        //SchulteTableAttentionTest
        public double ARTest { get; set; }
        public double VRTest { get; set; }
        public double PUTest { get; set; }


        //EmotionalIntelligenceResult
        public double MPTest { get; set; }
        public double MUTest { get; set; }
        public double VPTest { get; set; }
        public double VUTest { get; set; }
        public double VATest { get; set; }
        public double MAETest { get; set; }
        public double VAETest { get; set; }
        public double PATest { get; set; }
        public double UATest { get; set; }
        public double OUTest { get; set; }


        //IntellectionResult
        public double COTTest { get; set; }
        public double SPTest { get; set; }
        public double EbbinghouseTest { get; set; }
        public double EliminateUnnecessaryTest { get; set; }
        public double AnalogiesTest { get; set; }

        public static List<PatientViewModel> SetListOfPatientViewModel(List<Patient> patients)
        {
            var patientsList = new List<PatientViewModel>();
            foreach (var p in patients)
            {
                patientsList.Add(new PatientViewModel()
                {
                    Id = p.Id,
                    //RegistrationYear = p.RegistrationYear,
                    //FinalResult = p.FinalResult,

                    //memory
                    FirstTest = p.MemoryResult.TenWordsTest.FirstTest.Result,
                    SecondTest = p.MemoryResult.TenWordsTest.SecondTest.Result,
                    ThirdTest = p.MemoryResult.TenWordsTest.SecondTest.Result,
                    FourthTest = p.MemoryResult.TenWordsTest.FourthTest.Result,
                    FifthTest = p.MemoryResult.TenWordsTest.FifthTest.Result,
                    AfterFourtyMinutesTest = p.MemoryResult.TenWordsTest.AfterFourtyMinutesTest.Result,

                    FigurativeMemoryTest = p.MemoryResult.FigurativeMemoryTest.Result,

                    Time = p.MemoryResult.VisualMemoryTest.Time,
                    Result = p.MemoryResult.VisualMemoryTest.Result,

                    SemanticMemoryTest = p.MemoryResult.SemanticMemoryTest.Result,

                    VMemoryTest = p.MemoryResult.VMemoryTest.Result,

                    //attention
                    MunstAttentionTest = p.AttentionResult.MunstAttentionTest.Result,

                    EUTest = p.AttentionResult.CorTableAttentionTest.EUTest.Result,
                    CAVTest = p.AttentionResult.CorTableAttentionTest.CAVTest.Result,

                    ARTest = p.AttentionResult.SchulteTableAttentionTest.ARTest.Result,
                    VRTest = p.AttentionResult.SchulteTableAttentionTest.VRTest.Result,
                    PUTest = p.AttentionResult.SchulteTableAttentionTest.PUTest.Result,

                    //emotional
                    MPTest = p.EmotionalIntelligenceResult.MPTest.Result,
                    MUTest = p.EmotionalIntelligenceResult.MUTest.Result,
                    VPTest = p.EmotionalIntelligenceResult.VPTest.Result,
                    VUTest = p.EmotionalIntelligenceResult.VUTest.Result,
                    VATest = p.EmotionalIntelligenceResult.VATest.Result,
                    MAETest = p.EmotionalIntelligenceResult.MAETest.Result,
                    VAETest = p.EmotionalIntelligenceResult.VAETest.Result,
                    PATest = p.EmotionalIntelligenceResult.PATest.Result,
                    UATest = p.EmotionalIntelligenceResult.UATest.Result,
                    OUTest = p.EmotionalIntelligenceResult.OUTest.Result,

                    //intellection
                    COTTest = p.IntellectionResult.COTTest.Result,
                    SPTest = p.IntellectionResult.SPTest.Result,
                    EbbinghouseTest = p.IntellectionResult.EbbinghouseTest.Result,
                    EliminateUnnecessaryTest = p.IntellectionResult.EliminateUnnecessaryTest.Result,
                    AnalogiesTest = p.IntellectionResult.AnalogiesTest.Result
                });
            }

            return patientsList;
        }

        public static double[] GetPatientAttributes(PatientViewModel patient)
        {
            return new []
            {
                patient.FirstTest,
                patient.SecondTest,
                patient.ThirdTest,
                patient.FourthTest,
                patient.FifthTest,
                patient.AfterFourtyMinutesTest,
                patient.FigurativeMemoryTest,
                Math.Round(patient.Time / 60.0, 2), //time for visual memory test
                patient.Result,
                patient.SemanticMemoryTest,
                patient.VMemoryTest,
                patient.MunstAttentionTest,
                patient.EUTest,
                patient.CAVTest,
                patient.ARTest,
                patient.VRTest,
                patient.PUTest,
                patient.MPTest,
                patient.MUTest,
                patient.VPTest,
                patient.VUTest,
                patient.VATest,
                patient.MAETest,
                patient.VAETest,
                patient.PATest,
                patient.UATest,
                patient.OUTest,
                patient.COTTest,
                patient.SPTest,
                patient.EbbinghouseTest,
                patient.EliminateUnnecessaryTest,
                patient.AnalogiesTest
            };
        }
    }
}