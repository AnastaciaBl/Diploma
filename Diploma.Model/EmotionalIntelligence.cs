namespace Diploma.Model
{
    public class EmotionalIntelligence : Test
    {
        public TestResult MPTest { get; set; }
        public TestResult MUTest { get; set; }
        public TestResult VPTest { get; set; }
        public TestResult VUTest { get; set; }
        public TestResult VATest { get; set; }
        public TestResult MAETest { get; set; }
        public TestResult VAETest { get; set; }
        public TestResult PATest { get; set; }
        public TestResult UATest { get; set; }
        public TestResult OUTest { get; set; }

        public EmotionalIntelligence()
        {
            MPTest = new TestResult();
            MUTest = new TestResult();
            VPTest = new TestResult();
            VUTest = new TestResult();
            VATest = new TestResult();
            MAETest = new TestResult();
            VAETest = new TestResult();
            PATest = new TestResult();
            UATest = new TestResult();
            OUTest = new TestResult();
        }
    }
}
