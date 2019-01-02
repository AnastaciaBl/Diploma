namespace Diploma.Model
{
    public class Patient
    {
        public int RegistrationYear { get; set; }
        public string Name { get; set; }
        public double FinalResult { get; set; }
        public Memory MemoryResult { get; set; }
        public Attention AttentionResult { get; set; }
        public EmotionalIntelligence EmotionalIntelligenceResult { get; set; }
        public Intellection IntellectionResult { get; set; }
    }
}
