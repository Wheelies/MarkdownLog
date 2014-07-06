using MarkdownLog;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.MarkdownLog
{
    [TestClass]
    public class IosTableViewTests
    {
        [TestMethod]
        public void TestCanPlotDisclosureIndicators()
        {
            var tableView = new TableView
            {
                Sections = new[]
                {
                    new TableViewSection
                    {
                        Header = new IosTableViewHeaderCell {Text = "Mammals"}, Cells = new[]
                        {
                            new IosTableViewCell("Elephant", TableViewCellAccessory.DisclosureIndicator),
                            new IosTableViewCell("Giraffe", TableViewCellAccessory.DisclosureIndicator),
                            new IosTableViewCell("Monkey", TableViewCellAccessory.DisclosureIndicator),
                            new IosTableViewCell("Cat", TableViewCellAccessory.DisclosureIndicator)
                        }
                    },
                    new TableViewSection
                    {
                        Header = new IosTableViewHeaderCell {Text = "Reptiles"}, Cells = new[]
                        {
                            new IosTableViewCell("Lizard", TableViewCellAccessory.DisclosureIndicator),
                            new IosTableViewCell("Snake", TableViewCellAccessory.DisclosureIndicator),
                            new IosTableViewCell("Crocodile", TableViewCellAccessory.DisclosureIndicator)
                        }
                    }
                }
            };

            tableView.WriteToTrace();
        }

        [TestMethod]
        public void TestCanPlotWithoutHeader()
        {
            var tableView = new TableView
            {
                Sections = new[]
                {
                    new TableViewSection
                    {
                        Cells = new[]
                        {
                            new IosTableViewCell("Red"),
                            new IosTableViewCell("Orange"),
                            new IosTableViewCell("Yellow"),
                            new IosTableViewCell("Green"),
                            new IosTableViewCell("Blue")
                        }
                    }
                }
            };

            tableView.WriteToTrace();
        }

        [TestMethod]
        public void TestCanPlotMultipleSectionsWithOneSectionHavingNoHeader()
        {

            var tableView = new TableView
            {
                Sections = new[]
                {
                    new TableViewSection
                    {
                        Header = new IosTableViewHeaderCell {Text = "Header"},
                        Cells = new[]
                        {
                            new IosTableViewCell("One"),
                            new IosTableViewCell("Two"),
                            new IosTableViewCell("Three")
                        }
                    },
                    new TableViewSection
                    {
                        Cells = new[]
                        {
                            new IosTableViewCell("Cat"),
                            new IosTableViewCell("Dog"),
                            new IosTableViewCell("Fish")
                        }
                    }
                }
            };

            tableView.WriteToTrace();
        }

        [TestMethod]
        public void TestCanPlotCheckmarks()
        {
            var tableView = new TableView
            {
                Sections = new[]
                {
                    new TableViewSection
                    {
                        Header = new IosTableViewHeaderCell {Text = "Pizza Toppings"}, Cells = new[]
                        {
                            new IosTableViewCell("Extra cheese", TableViewCellAccessory.Checkmark),
                            new IosTableViewCell("Pepperoni", TableViewCellAccessory.None),
                            new IosTableViewCell("Black olive", TableViewCellAccessory.Checkmark),
                            new IosTableViewCell("Sausage", TableViewCellAccessory.Checkmark),
                            new IosTableViewCell("Mushroom", TableViewCellAccessory.Checkmark),
                            new IosTableViewCell("Pepper", TableViewCellAccessory.None)
                        }
                    }
                }
            };

            tableView.WriteToTrace();
        }

        [TestMethod]
        public void TestCanPlotWithNoSections()
        {
            var tableView = new TableView {Sections = new TableViewSection[0]};
            tableView.WriteToTrace();
        }

        [TestMethod]
        public void TestCanPlotSingleSectionWithNoHeaderOrCells()
        {

            var tableView = new TableView {Sections = new[] {new TableViewSection()}};
            tableView.WriteToTrace();
        }

    }
}