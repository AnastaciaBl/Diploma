﻿namespace Diploma.Model.AttentionTests
{
    public class SchulteTableAttention
    {
        public TestResult ARTest { get; set; }
        public TestResult VRTest { get; set; }
        public TestResult PUTest { get; set; }

        public SchulteTableAttention()
        {
            ARTest = new TestResult();
            VRTest = new TestResult();
            PUTest = new TestResult();
        }
    }
}
