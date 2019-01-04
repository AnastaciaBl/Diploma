namespace Diploma.Model.MemoryTests
{
    public class TenWords
    {
        public TestResult FirstTest { get; set; }
        public TestResult SecondTest { get; set; }
        public TestResult ThirdTest { get; set; }
        public TestResult FourthTest { get; set; }
        public TestResult FifthTest { get; set; }
        public TestResult AfterFourtyMinutesTest { get; set; }

        public TenWords()
        {
            FirstTest = new TestResult();
            SecondTest = new TestResult();
            ThirdTest = new TestResult();
            FourthTest = new TestResult();
            FifthTest = new TestResult();
            AfterFourtyMinutesTest = new TestResult();
        }
    }
}
