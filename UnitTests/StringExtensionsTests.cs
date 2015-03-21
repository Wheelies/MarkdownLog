using MarkdownLog;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
using NUnit.Framework;

namespace UnitTests.MarkdownLog
{
    [TestClass]
    public class StringExtensionsTests
    {
        [TestMethod]
        public void TestMarkdownCharactersWithinTextAreEscaped()
        {
            Assert.AreEqual(@"this is some \*text\*", "this is some *text*".EscapeMarkdownCharacters(), "asterisks are escaped");
            Assert.AreEqual(@"this is some \*\*text\*\*", "this is some **text**".EscapeMarkdownCharacters(), "double asterisk is escaped");
            Assert.AreEqual(@"this is some \_text\_", "this is some _text_".EscapeMarkdownCharacters(), "underscore is escaped");
            Assert.AreEqual(@"this is some \_\_text\_\_", "this is some __text__".EscapeMarkdownCharacters(), "double underscore is escaped");
            Assert.AreEqual(@"this is some \`text\`", "this is some `text`".EscapeMarkdownCharacters(), "backtick is escaped");
            Assert.AreEqual(@"this is a backslash \\", @"this is a backslash \".EscapeMarkdownCharacters(), "backslash is escaped");
        }

        [TestMethod]
        public void TestHeaderCharacterAtBeginningAreEscaped()
        {
            Assert.AreEqual(@"\#this looks like a header", "#this looks like a header".EscapeMarkdownCharacters(), "hash is escaped if at start");
            Assert.AreEqual(@"this is a hash #, and another #", "this is a hash #, and another #".EscapeMarkdownCharacters(), "hash is not escaped elsewhere");

            Assert.AreEqual(@"\##looks like a second level header", "##looks like a second level header".EscapeMarkdownCharacters(), "two hashes at begining are escaped");
            Assert.AreEqual(@"\###looks like a third level header", "###looks like a third level header".EscapeMarkdownCharacters(), "three hashes at begining are escaped");
            Assert.AreEqual(@"\####looks like a fourth level header", "####looks like a fourth level header".EscapeMarkdownCharacters(), "four hashes at begining are escaped");
            Assert.AreEqual(@"\#####looks like a fifth level header", "#####looks like a fifth level header".EscapeMarkdownCharacters(), "five hashes at begining are escaped");
            Assert.AreEqual(@"\######looks like a sixth level header", "######looks like a sixth level header".EscapeMarkdownCharacters(), "six hashes at begining are escaped");
        }

        [TestMethod]
        public void TestFullStopAfterNumbersIsEscaped()
        {
            Assert.AreEqual(@"1\. This looks like a list", "1. This looks like a list".EscapeMarkdownCharacters(), "A full-stop (period) following one number will be escaped");
            Assert.AreEqual(@"12\. This looks like a list", "12. This looks like a list".EscapeMarkdownCharacters(), "A full-stop (period) following two numbers will be escaped");
            Assert.AreEqual(@"123\. This looks like a list", "123. This looks like a list".EscapeMarkdownCharacters(), "A full-stop (period) following three numbers will be escaped");
            Assert.AreEqual(@"1234\. This looks like a list", "1234. This looks like a list".EscapeMarkdownCharacters(), "A full-stop (period) following four numbers will be escaped");
        }

        [TestMethod]
        public void TestSplitByLines()
        {
            var lines = "apple\nbanana\ncherry".SplitByLine();
            Assert.AreEqual(3, lines.Count);
            Assert.AreEqual("apple", lines[0]);
            Assert.AreEqual("banana", lines[1]);
            Assert.AreEqual("cherry", lines[2]);
        }

        [TestMethod]
        public void TestSplitByLinesHandlesAllLineEndings()
        {
            Assert.AreEqual(3, "a\rb\rc".SplitByLine().Count);
            Assert.AreEqual(3, "a\nb\nc".SplitByLine().Count);
            Assert.AreEqual(3, "a\r\nb\r\nc".SplitByLine().Count);
            Assert.AreEqual(3, "a\n\rb\n\rc".SplitByLine().Count);
        }

        [TestMethod]
        public void TestSplitByLinesHandlesMixedLineEndings()
        {
            Assert.AreEqual(5, "a\rb\nc\r\nd\n\re".SplitByLine().Count);
        }

    }
}
