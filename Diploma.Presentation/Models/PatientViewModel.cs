namespace Diploma.Presentation.Models
{
    public class PatientViewModel
    {
        public int Id { get; set; }
        public int RegistrationYear { get; set; }
        public string Name { get; set; }
        public double FinalResult { get; set; }

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
    }
}