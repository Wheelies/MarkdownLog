using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MarkdownLog;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;

namespace UnitTests.MarkdownLog
{
    [TestClass]
    public class TableTests
    {
        [TestMethod]
        public void TestCanProduceTable()
        {
            var table = new Table
            {
                Columns = new[]
                {
                    new TableColumn{HeaderCell = new TableCell {Text = "Year"}},
                    new TableColumn{HeaderCell = new TableCell {Text = "Album"}},
                    new TableColumn{HeaderCell = new TableCell {Text = "Song Count"}},
                    new TableColumn{HeaderCell = new TableCell {Text = "Rating"}}
                },
                Rows = new[]
                {
                    new TableRow
                    {
                        Cells = new[]
                        {
                            new TableCell {Text = "1991"},
                            new TableCell {Text = "Out of Time"},
                            new TableCell {Text = "11"},
                            new TableCell {Text = "* * * *"}
                        }
                    },
                    new TableRow
                    {
                        Cells = new[]
                        {
                            new TableCell {Text = "1992"},
                            new TableCell {Text = "Automatic for the People"},
                            new TableCell {Text = "12"},
                            new TableCell {Text = "* * * * *"}
                        }
                    },
                    new TableRow
                    {
                        Cells = new[]
                        {
                            new TableCell {Text = "1994"},
                            new TableCell {Text = "Monster"},
                            new TableCell {Text = "12"},
                            new TableCell {Text = "* * *"}
                        }
                    }
                }
            };

            table.AssertOutputEquals(
                "     Year | Album                    | Song Count | Rating   "+Environment.NewLine +
                "     ---- | ------------------------ | ---------- | --------- "+Environment.NewLine +
                "     1991 | Out of Time              | 11         | * * * *  "+Environment.NewLine +
                "     1992 | Automatic for the People | 12         | * * * * *"+Environment.NewLine +
                "     1994 | Monster                  | 12         | * * *    "+Environment.NewLine
                ,
                "<table>\n" +
                "<thead>\n" +
                "<tr>\n" +
                "	<th>Year</th>\n" +
                "	<th>Album</th>\n" +
                "	<th>Song Count</th>\n" +
                "	<th>Rating</th>\n" +
                "</tr>\n" +
                "</thead>\n" +
                "<tbody>\n" +
                "<tr>\n" +
                "	<td>1991</td>\n" +
                "	<td>Out of Time</td>\n" +
                "	<td>11</td>\n" +
                "	<td>* * * *</td>\n" +
                "</tr>\n" +
                "<tr>\n" +
                "	<td>1992</td>\n" +
                "	<td>Automatic for the People</td>\n" +
                "	<td>12</td>\n" +
                "	<td>* * * * *</td>\n" +
                "</tr>\n" +
                "<tr>\n" +
                "	<td>1994</td>\n" +
                "	<td>Monster</td>\n" +
                "	<td>12</td>\n" +
                "	<td>* * *</td>\n" +
                "</tr>\n" +
                "</tbody>\n" +
                "</table>\n");
        }

        [TestMethod]
        public void TestHeadersAreAutomaticallyAddedIfNotSpecified()
        {
            var table = new Table
            {
                Rows = new[]
                {
                    new TableRow
                    {
                        Cells = new[]
                        {
                            new TableCell {Text = "Avatar"},
                            new TableCell {Text = "2009"},
                            new TableCell {Text = "$2,782,275,172"}
                        }
                    },
                    new TableRow
                    {
                        Cells = new[]
                        {
                            new TableCell {Text = "Titanic"},
                            new TableCell {Text = "1997"},
                            new TableCell {Text = "$2,186,772,302"}
                        }
                    },
                    new TableRow
                    {
                        Cells = new[]
                        {
                            new TableCell {Text = "The Avengers"},
                            new TableCell {Text = "2012"},
                            new TableCell {Text = "$1,518,594,910"}
                        }
                    }
                }
            };

            table.AssertOutputEquals(
                "     A            | B    | C             "+Environment.NewLine +
                "     ------------ | ---- | -------------- "+Environment.NewLine +
                "     Avatar       | 2009 | $2,782,275,172"+Environment.NewLine +
                "     Titanic      | 1997 | $2,186,772,302"+Environment.NewLine +
                "     The Avengers | 2012 | $1,518,594,910"+Environment.NewLine);
        }


        [TestMethod]
        public void TestColumnContentsCanBeAligned()
        {
            var table = new Table
            {
                Columns = new[]
                {
                    new TableColumn{HeaderCell = new TableCell {Text = "Animal"}, Alignment = TableColumnAlignment.Left},
                    new TableColumn{HeaderCell = new TableCell {Text = "Good Pet?"},Alignment = TableColumnAlignment.Center},
                    new TableColumn{HeaderCell = new TableCell {Text = "Lifespan (years)"}, Alignment = TableColumnAlignment.Right}
                },
                Rows = new[]
                {
                    new TableRow
                    {
                        Cells = new[]
                        {
                            new TableCell {Text = "Cat"},
                            new TableCell {Text = "[X]"},
                            new TableCell {Text = "14"}
                        }
                    },
                    new TableRow
                    {
                        Cells = new[]
                        {
                            new TableCell {Text = "Mouse"},
                            new TableCell {Text = "[X]"},
                            new TableCell {Text = "2"}
                        }
                    },
                    new TableRow
                    {
                        Cells = new[]
                        {
                            new TableCell {Text = "Elephant"},
                            new TableCell {Text = "[ ]"},
                            new TableCell {Text = "65"}
                        }
                    }
                }
            };

            table.AssertOutputEquals(
                "     Animal   | Good Pet? | Lifespan (years)"+Environment.NewLine +
                "    :-------- |:---------:| ----------------:"+Environment.NewLine +
                "     Cat      |    [X]    |               14"+Environment.NewLine +
                "     Mouse    |    [X]    |                2"+Environment.NewLine +
                "     Elephant |    [ ]    |               65"+Environment.NewLine);
        }

        [TestMethod]
        public void TestTableCanBeBuiltAutomaticallyFromEnumerable()
        {
            var data = new[]
            {
                new{Name = "Meryl Streep", Nominations = 18, Awards=3},
                new{Name = "Katharine Hepburn", Nominations = 12, Awards=4},
                new{Name = "Jack Nicholson", Nominations = 12, Awards=3},
                new{Name = "Bette Davis", Nominations = 10, Awards=2},
                new{Name = "Laurence Olivier", Nominations = 10, Awards=1},
                new{Name = "Spencer Tracy", Nominations = 9, Awards=2}
                
            };

            data.ToMarkdownTable().AssertOutputEquals(
                "     Name              | Nominations | Awards"+Environment.NewLine +
                "     ----------------- | -----------:| ------:"+Environment.NewLine +
                "     Meryl Streep      |          18 |      3"+Environment.NewLine +
                "     Katharine Hepburn |          12 |      4"+Environment.NewLine +
                "     Jack Nicholson    |          12 |      3"+Environment.NewLine +
                "     Bette Davis       |          10 |      2"+Environment.NewLine +
                "     Laurence Olivier  |          10 |      1"+Environment.NewLine +
                "     Spencer Tracy     |           9 |      2"+Environment.NewLine);
        }

        [TestMethod]
        public void TestColumnsCanBeSpecifiedForAutomaticallyBuiltTables()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            new DirectoryInfo(path).GetFiles()
                .ToMarkdownTable(i => i.Name, i => i.Length, i => (int)(DateTime.Now - i.CreationTime).TotalDays)
                .WithHeaders("Name", "Size (Bytes)", "Age (Days)")
                .WriteToTrace();
        }

        [TestMethod]
        public void TestColumnsContainingOnlyNumbersOrBlanksOrNullsIsAutomaticallyAlignedRight()
        {
            var data = new Dictionary<string, object>
            {
                {"Pi", "3.14"},
                {"Blank", ""},
                {"Null", null},
                {"e", 2.718m},
                {"sqrt(2)", 1.414f},
                {"Gelford's constant", 23.141},
            };

            data.ToMarkdownTable().AssertOutputEquals(
                "     Key                | Value"+Environment.NewLine +
                "     ------------------ | -----:"+Environment.NewLine +
                "     Pi                 |  3.14"+Environment.NewLine +
                "     Blank              |      "+Environment.NewLine +
                "     Null               |      "+Environment.NewLine +
                "     e                  |  2.72"+Environment.NewLine +
                "     sqrt(2)            |  1.41"+Environment.NewLine +
                "     Gelford's constant | 23.14"+Environment.NewLine);
        }

        [TestMethod]
        public void TestDecimalValuesAreFormattedToTwoDecimalPlaces()
        {
            var squareRoots = Enumerable.Range(0, 12).Select(x => new {X = x*10, Sqrt = Math.Sqrt(x*10)});
            
            squareRoots.ToMarkdownTable().WriteToTrace();
        }

        [TestMethod]
        public void TestDateValuesAreFormattedToRfc1123()
        {
            new[]
            {
                new {Name = "Edward VII", Coronation = new DateTime(1902, 8, 9)},
                new {Name = "George V", Coronation = new DateTime(1911, 6, 22)},
                new {Name = "George VI", Coronation = new DateTime(1937, 5, 12)},
                new {Name = "Elizabeth II", Coronation = new DateTime(1953, 6, 2)}
            }.ToMarkdownTable().WriteToTrace();
        }


        [TestMethod]
        public void TestLineBreaksTabsAndSpecialCharactersAreEncodedInTableCells()
        {
            var books = new[]
            {
                new
                {
                    Type = "Unix style Line Break",
                    Content = "First Line\nSecond Line",
                },
                new
                {
                    Type = "ZX Spectrum style Line Break",
                    Content = "First Line\rSecond Line",
                },
                new
                {
                    Type = "Acorn BBC Spooled output Line Break",
                    Content = "First Line\n\rSecond Line",
                },
                new
                {
                    Type = "Windows style output Line Break",
                    Content = "First Line\r\nSecond Line",
                },
                new
                {
                    Type = "Tab characters",
                    Content = "Column1\tColumn2",
                },
                new
                {
                    Type = "Backslash characters",
                    Content = @"\\myserver\path\to\file.txt",
                }
            };

            books.ToMarkdownTable().WriteToTrace();
        }

        private class Person
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public IList<Person> Children { get; set; }

            public Person()
            {
                Children = new Person[0];
            }
        }

        [TestMethod]
        public void TestCanConfigureOutputOfCollectionsUsingExtensionMethod()
        {
            var bart = new Person { Name = "Bart Simpson", Age = 10 };
            var lisa = new Person { Name = "Lisa Simpson", Age = 8 };
            var maggie = new Person { Name = "Maggie Simpson", Age = 1 };
            var homer = new Person { Name = "Homer Simpson", Age = 36, Children = new[] { bart, lisa, maggie } };
            var marge = new Person { Name = "Marge Simpson", Age = 35, Children = new[] { bart, lisa, maggie } };
            var milhouse = new Person { Name = "Milhouse Van Houten ", Age = 10 };

            var simpsons = new[] { homer, marge, bart, lisa, maggie, milhouse };

            var md = new MarkdownContainer();
            md.Append("Table built with no options".ToMarkdownHeader());
            md.Append(simpsons.ToMarkdownTable());

            md.Append("Table built with Default options".ToMarkdownHeader());
            md.Append(simpsons.ToMarkdownTable(TableOptions.Default));

            md.Append("Table built with ExcludeCollectionProperties options".ToMarkdownHeader());
            md.Append(simpsons.ToMarkdownTable(TableOptions.ExcludeCollectionProperties));

            Console.WriteLine(md);
            
        }

    }
}