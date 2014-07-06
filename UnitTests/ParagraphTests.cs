using MarkdownLog;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.MarkdownLog
{
    [TestClass]
    public class ParagraphTests
    {
        [TestMethod]
        public void TestParagraphsCanBeUsed()
        {
            var paragraph = new Paragraph("The quick brown fox jumps over the lazy dog.");
            
            paragraph.WriteToTrace();
        }

        [TestMethod]
        public void TestLongParagraphsAreWrapped()
        {
            var paragraph = new Paragraph("Lolita, light of my life, fire of my loins. My sin, my soul. Lo-lee-ta: the tip of the tongue taking a trip of three steps down the palate to tap, at three, on the teeth. Lo. Lee. Ta.");
            
            paragraph.WriteToTrace();
        }

        [TestMethod]
        public void TestWrappingCanBeDisabled()
        {
            var paragraph = new Paragraph("Whoever fights monsters should see to it that in the process he does not become a monster. And if you gaze long enough into an abyss, the abyss will gaze back into you.")
            {
                WordWrap = false
            };

            paragraph.WriteToTrace();
        }

        [TestMethod]
        public void TestWrappingColumnCanBeSpecified()
        {
            var paragraph = new Paragraph("Most people die of a sort of creeping common sense, and discover when it is too late that the only things one never regrets are one's mistakes.")
            {
                WordWrapColumn = 20
            };

            paragraph.WriteToTrace();
        }

        [TestMethod]
        public void TestLineBreaksAreHandled()
        {
            var paragraph = new Paragraph("I wandered lonely as a cloud\r\n" +
                                          "That floats on high o'er vales and hills,\r\n" +
                                          "When all at once I saw a crowd,\r\n" +
                                          "A host, of golden daffodils;\r\n" +
                                          "Beside the lake, beneath the trees,\r\n" +
                                          "Fluttering and dancing in the breeze.");


            paragraph.AssertOutputEquals(
                "I wandered lonely as a cloud  \r\n" +
                "That floats on high o'er vales and hills,  \r\n" +
                "When all at once I saw a crowd,  \r\n" +
                "A host, of golden daffodils;  \r\n" +
                "Beside the lake, beneath the trees,  \r\n" +
                "Fluttering and dancing in the breeze.\r\n"
                ,
                "<p>I wandered lonely as a cloud<br />\n" +
                "That floats on high o'er vales and hills,<br />\n" +
                "When all at once I saw a crowd,<br />\n" +
                "A host, of golden daffodils;<br />\n" +
                "Beside the lake, beneath the trees,<br />\n" +
                "Fluttering and dancing in the breeze.</p>\n");
        }

        [TestMethod]
        public void TestDifferentTypesOfLineBreaksAreHandled()
        {
            var paragraph = new Paragraph("Unix style:\n" +
                                          "ZX Spectrum style:\r" +
                                          "Acorn BBC Spooled output:\n\r" +
                                          "Windows style:\r\n");

            paragraph.AssertOutputEquals(
                "Unix style:  \r\n" +
                "ZX Spectrum style:  \r\n" +
                "Acorn BBC Spooled output:  \r\n" +
                "Windows style:\r\n"
                ,
                "<p>Unix style:<br />\n" +
                "ZX Spectrum style:<br />\n" +
                "Acorn BBC Spooled output:<br />\n" +
                "Windows style:</p>\n");
        }

        [TestMethod]
        public void TestSpecialCharactersAreEscapedAutomatically()
        {
            new Paragraph("#This looks like a header#").WriteToTrace();

            new Paragraph("Special markdown symbols, like *this* and _that_, will be escaped so that the HTML is a faithful reproduction of the input").WriteToTrace();

            new Paragraph("1984. A great book!").WriteToTrace();
        }

    }
}