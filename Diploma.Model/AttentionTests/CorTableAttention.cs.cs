namespace Diploma.Model.AttentionTests
{
    public class CorTableAttention
    {
        public TestResult EUTest { get; set; }
        public TestResult CAVTest { get; set; }

        public CorTableAttention()
        {
            EUTest = new TestResult();
            CAVTest = new TestResult();
        }
    }
}
