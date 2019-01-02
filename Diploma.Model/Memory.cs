using Diploma.Model.MemoryTests;

namespace Diploma.Model
{
    public class Memory : Test
    {
        public TenWords TenWordsTest { get; set; }
        public TestResult FigurativeMemoryTest { get; set; }
        public VisualMemory VisualMemoryTest { get; set; }
        public TestResult SemanticMemoryTest { get; set; }
        public TestResult VMemoryTest { get; set; }
    }
}
