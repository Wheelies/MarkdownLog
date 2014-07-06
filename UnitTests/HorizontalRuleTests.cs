using MarkdownLog;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.MarkdownLog
{
    [TestClass]
    public class HorizontalRuleTests
    {
        [TestMethod]
        public void TestHorizontalRuleCanBeDrawn()
        {
            var rule = new HorizontalRule();

            rule.AssertOutputEquals(
                "--------------------------------------------------------------------------------\r\n"
                ,
                "<hr />\n");
        }

    }
}