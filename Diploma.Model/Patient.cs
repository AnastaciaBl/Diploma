using System.Collections.Generic;

namespace Diploma.Model
{
    public class Patient
    {
        public int Id { get; set; }
        public int RegistrationYear { get; set; }
        public string Name { get; set; }
        public double FinalResult { get; set; }
        public Memory MemoryResult { get; set; }
        public Attention AttentionResult { get; set; }
        public EmotionalIntelligence EmotionalIntelligenceResult { get; set; }
        public Intellection IntellectionResult { get; set; }
        //public List<double> TestsValues { get; set; }

        public Patient()
        {
            MemoryResult = new Memory();
            AttentionResult = new Attention();
            EmotionalIntelligenceResult = new EmotionalIntelligence();
            IntellectionResult = new Intellection();
            //TestsValues = new List<double>();
        }

        public static double[] GetPatientAttributes(Patient p)
        {
            return new[]
            {
                    //memory
                    p.MemoryResult.TenWordsTest.FirstTest.Result,
                    p.MemoryResult.TenWordsTest.SecondTest.Result,
                    p.MemoryResult.TenWordsTest.SecondTest.Result,
                    p.MemoryResult.TenWordsTest.FourthTest.Result,
                    p.MemoryResult.TenWordsTest.FifthTest.Result,
                    p.MemoryResult.TenWordsTest.AfterFourtyMinutesTest.Result,

                    p.MemoryResult.FigurativeMemoryTest.Result,

                    p.MemoryResult.VisualMemoryTest.Time,
                    p.MemoryResult.VisualMemoryTest.Result,

                    p.MemoryResult.SemanticMemoryTest.Result,

                    p.MemoryResult.VMemoryTest.Result,

                    //attention
                    p.AttentionResult.MunstAttentionTest.Result,

                    p.AttentionResult.CorTableAttentionTest.EUTest.Result,
                    p.AttentionResult.CorTableAttentionTest.CAVTest.Result,

                    p.AttentionResult.SchulteTableAttentionTest.ARTest.Result,
                    p.AttentionResult.SchulteTableAttentionTest.VRTest.Result,
                    p.AttentionResult.SchulteTableAttentionTest.PUTest.Result,

                    //emotional
                    p.EmotionalIntelligenceResult.MPTest.Result,
                    p.EmotionalIntelligenceResult.MUTest.Result,
                    p.EmotionalIntelligenceResult.VPTest.Result,
                    p.EmotionalIntelligenceResult.VUTest.Result,
                    p.EmotionalIntelligenceResult.VATest.Result,
                    p.EmotionalIntelligenceResult.MAETest.Result,
                    p.EmotionalIntelligenceResult.VAETest.Result,
                    p.EmotionalIntelligenceResult.PATest.Result,
                    p.EmotionalIntelligenceResult.UATest.Result,
                    p.EmotionalIntelligenceResult.OUTest.Result,

                    //intellection
                    p.IntellectionResult.COTTest.Result,
                    p.IntellectionResult.SPTest.Result,
                    p.IntellectionResult.EbbinghouseTest.Result,
                    p.IntellectionResult.EliminateUnnecessaryTest.Result,
                    p.IntellectionResult.AnalogiesTest.Result
            };
        }
    }
}