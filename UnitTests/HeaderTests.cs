using MarkdownLog;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
                "Frankenstein;\r\n" +
                "or,\r\n" +
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