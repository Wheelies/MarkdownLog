using MarkdownLog;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.MarkdownLog
{
    [TestClass]
    public class ListTests
    {
        [TestMethod]
        public void TestListsCanHaveBulletPoints()
        {
            var list = new BulletedList(
                "Apple",
                "Orange",
                "Banana",
                "Kiwi",
                "Plum");
            
            list.AssertOutputEquals(
                "   * Apple\r\n" +
                "   * Orange\r\n" +
                "   * Banana\r\n" +
                "   * Kiwi\r\n" +
                "   * Plum\r\n"
                ,
                "<ul>\n" +
                "<li>Apple</li>\n" +
                "<li>Orange</li>\n" +
                "<li>Banana</li>\n" +
                "<li>Kiwi</li>\n" +
                "<li>Plum</li>\n" +
                "</ul>\n");
        }

        [TestMethod]
        public void TestListsCanBeNumbered()
        {
            var list = new NumberedList(
                "The Beatles",
                "Elvis Presley",
                "Michael Jackson",
                "Madonna",
                "Elton John");

            list.AssertOutputEquals(
                "   1. The Beatles\r\n" +
                "   2. Elvis Presley\r\n" +
                "   3. Michael Jackson\r\n" +
                "   4. Madonna\r\n" +
                "   5. Elton John\r\n"
                ,
                "<ol>\n" +
                "<li>The Beatles</li>\n" +
                "<li>Elvis Presley</li>\n" +
                "<li>Michael Jackson</li>\n" +
                "<li>Madonna</li>\n" +
                "<li>Elton John</li>\n" +
                "</ol>\n");
        }
        
        [TestMethod]
        public void TestLongListItemsAreAutomaticallyWrappedAt80CharactersByDefault()
        {
            var list = new NumberedList(
                "Every body persists in its state of being at rest or of moving uniformly straight forward, except insofar as it is compelled to change its state by force impressed.",
                "The change of momentum of a body is proportional to the impulse impressed on the body, and happens along the straight line on which that impulse is impressed.",
                "To every action there is always opposed an equal reaction: or the mutual actions of two bodies upon each other are always equal, and directed to contrary parts.");
            list.WriteToTrace();
        }

        [TestMethod]
        public void TestWordWrapColumnCanBeSpecified()
        {
            var list = new NumberedList(
                "A straight line segment can be drawn joining any two points.",
                "Any straight line segment can be extended indefinitely in a straight line.",
                "Given any straight line segment, a circle can be drawn having the segment as radius and one endpoint as center.",
                "All right angles are congruent.",
                "If two lines are drawn which intersect a third in such a way that the sum of the inner angles on one side is less than two right angles, then the two lines inevitably must intersect each other on that side if extended far enough. This postulate is equivalent to what is known as the parallel postulate.")
            {
                WordWrapColumn = 40
            };
            list.WriteToTrace();
        }

        [TestMethod]
        public void TestSpecialMarkDownCharactersAreAutomaticallyEncoded()
        {
            var list = new BulletedList(
                @"The underscore (_), backtick (`), asterisk (*) and backslash (\) have special meanings in Unicode",
                "These will be encoded by default so there are no surprises caused by your automatically generated Markdown containing special characters",
                "For example, variable_names_with_underscores and formula x = 1*2**3**4");
            list.WriteToTrace();
        }

        [TestMethod]
        public void TestCanProduceListFromAnyEnumerable()
        {
            "Mathematical constants".ToMarkdownHeader().WriteToTrace();
            new[] {3.14, 2.718, 1.618, 0.577215}.ToMarkdownBulettedList().WriteToTrace();

            new[] { "John", "Paul", "Ringo", "George" }.ToMarkdownBulettedList().WriteToTrace();

            new[] { "Mercury", "Venus", "Earth", "Mars", "Jupiter", "Saturn", "Uranus", "Neptune" }.ToMarkdownNumberedList().WriteToTrace();
        }

        [TestMethod]
        public void TestListCanHaveNoItems()
        {
            var list = new BulletedList();
            list.WriteToTrace();
        }
        
    }
}