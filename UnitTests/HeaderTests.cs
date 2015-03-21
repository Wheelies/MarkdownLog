using MarkdownLog;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
using System;

namespace UnitTests.MarkdownLog
{
    [TestClass]
    public class HeaderTests
    {
        [TestMethod]
        public void TestHeadersCanBeUsed()
        {
            var header = new Header("The Origin of the Species");
            
            header.WriteToTrace();
        }

        [TestMethod]
        public void TestSubHeadersCanBeUsed()
        {
            var subHeader = new SubHeader("By Means of Natural Selection");

            subHeader.WriteToTrace();
        }

        [TestMethod]
        public void TestLineBreaksWillProduceMultipleHeaders()
        {
            var header = new Header(
                "Frankenstein;"+Environment.NewLine +
                "or,"+Environment.NewLine +
                "The Modern Prometheus");

            header.WriteToTrace();
        }

        [TestMethod]
        public void TestSpecialCharactersInHeaderAreEscaped()
        {
            var header = new Header("Value of _x = 245 = 5 *7* 7 \\");

            header.WriteToTrace();
        }

    }
}