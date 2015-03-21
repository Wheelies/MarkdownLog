using MarkdownLog;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
using System;

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
                "--------------------------------------------------------------------------------"+Environment.NewLine
                ,
                "<hr />\n");
        }

    }
}