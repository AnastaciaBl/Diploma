namespace Diploma.Model
{
    public class Intellection : Test
    {
        public TestResult COTTest { get; set; }
        public TestResult SPTest { get; set; }
        public TestResult EbbinghouseTest { get; set; }
        public TestResult EliminateUnnecessaryTest { get; set; }
        public TestResult AnalogiesTest { get; set; }

        public Intellection()
        {
            COTTest = new TestResult();
            SPTest = new TestResult();
            EbbinghouseTest = new TestResult();
            EliminateUnnecessaryTest = new TestResult();
            AnalogiesTest = new TestResult();
        }
    }
}
