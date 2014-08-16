using MarkdownLog;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.MarkdownLog
{
    [TestClass]
    public class BlockquoteTests
    {
        [TestMethod]
        public void TestCanIncorporateTextInBlockQuote()
        {
            var blockQuote = new Blockquote();
            blockQuote.Append("We are what we repeatedly do. Excellence, therefore, is not an act but a habit.");

            blockQuote.AssertOutputEquals(
                "> We are what we repeatedly do. Excellence, therefore, is not an act but a habit.\r\n"
                ,
                "<blockquote>\n" +
                "<p>We are what we repeatedly do. Excellence, therefore, is not an act but a habit.</p>\n" +
                "</blockquote>\n");
        }

        [TestMethod]
        public void TestLineBreaksAreAutomaticallyHandled()
        {
            var blockQuote = new Blockquote();
            blockQuote.Append(@"You've gotta dance like there's nobody watching,
Love like you'll never be hurt,
Sing like there's nobody listening,
And live like it's heaven on earth.");

            blockQuote.WriteToTrace();
        }

        [TestMethod]
        public void TestOtherElementsCanBeIncorporatedIntoBlockquotes()
        {
            var blockQuote = new Blockquote();
            blockQuote.Append(new HorizontalRule());
            blockQuote.Append(new Header("COMPUTING MACHINERY AND INTELLIGENCE"));
            blockQuote.Append(new SubHeader("By A. M. Turing."));
            blockQuote.Append(new Paragraph("..."));
            blockQuote.Append(new Paragraph("The idea behind digital computers may be explained by saying that these machines are intended to carry out any operations which could be done by a human computer. The human computer is supposed to be following fixed rules; he has no authority to deviate from them in any detail. We may suppose that these rules are supplied in a book, which is altered whenever he is put on to a new job. He has also an unlimited supply of paper on which he does his calculations. He may also do his multiplications and additions on a \"desk machine,\" but this is not important."));
            blockQuote.Append(new Paragraph("If we use the above explanation as a definition we shall be in danger of circularity of argument. We avoid this by giving an outline. of the means by which the desired effect is achieved. A digital computer can usually be regarded as consisting of three parts:"));
            blockQuote.Append(new NumberedList("Store", "Executive unit", "Control"));

            blockQuote.WriteToTrace();
        }

        [TestMethod]
        public void TestListsWithWordWrapWillAlsoWork()
        {
            var blockQuote = new Blockquote();

            blockQuote.Append(new NumberedList(
                "Congress shall make no law respecting an establishment of religion, or prohibiting the free exercise thereof; or abridging the freedom of speech, or of the press; or the right of the people peaceably to assemble, and to petition the Government for a redress of grievances.",
                "A well regulated Militia, being necessary to the security of a free State, the right of the people to keep and bear Arms, shall not be infringed.",
                "No Soldier shall, in time of peace be quartered in any house, without the consent of the Owner, nor in time of war, but in a manner to be prescribed by law."));

            blockQuote.WriteToTrace();
        }
    }
}