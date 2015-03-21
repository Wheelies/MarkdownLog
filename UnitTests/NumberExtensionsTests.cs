using MarkdownLog;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
using NUnit.Framework;

namespace UnitTests.MarkdownLog
{
    [TestClass]
    public class NumberExtensionsTests
    {
        [TestMethod]
        public void TestToColumnTitle()
        {
            Assert.AreEqual("A", 0.ToColumnTitle());
            Assert.AreEqual("B", 1.ToColumnTitle());
            Assert.AreEqual("C", 2.ToColumnTitle());

            Assert.AreEqual("Z", 25.ToColumnTitle());
            Assert.AreEqual("AA", 26.ToColumnTitle());
            Assert.AreEqual("AB", 27.ToColumnTitle());
            Assert.AreEqual("AC", 28.ToColumnTitle());

            Assert.AreEqual("AZ", 51.ToColumnTitle());
            Assert.AreEqual("BA", 52.ToColumnTitle());

            Assert.AreEqual("YZ", 675.ToColumnTitle());
            Assert.AreEqual("ZA", 676.ToColumnTitle());

            Assert.AreEqual("ZZ", 701.ToColumnTitle());
            Assert.AreEqual("AAA", 702.ToColumnTitle());
            Assert.AreEqual("AAB", 703.ToColumnTitle());
        }        
    }
}