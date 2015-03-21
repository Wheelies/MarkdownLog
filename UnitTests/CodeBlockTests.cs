using MarkdownLog;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
using System;

namespace UnitTests.MarkdownLog
{
    [TestClass]
    public class CodeBlockTests
    {
        [TestMethod]
        public void TestCodeBlockCanBeCreated()
        {
            var code = new CodeBlock(@"
let rec qsort(xs : List<int>) =
  match xs with
  | [] -> []
  | x :: xs ->
      let smaller = qsort (xs |> List.filter(fun e -> e <= x))
      let larger = qsort (xs |> List.filter(fun e -> e >= x))
      smaller @ [x] @ larger
");
            code.WriteToTrace();
        }

        [TestMethod]
        public void TestCodeBlockCanBeBuiltByLine()
        {
            var code = new CodeBlock();
            code.AppendLine(@"m-config.      symbol         operations     final m-config.");
            code.AppendLine();
            code.AppendLine(@"             /  None              P0                b");
            code.AppendLine(@"   b        <    0             R, R, P1             b");
            code.AppendLine(@"             \   1             R, R, P0             b");

            code.AssertOutputEquals(
                "    m-config.      symbol         operations     final m-config."+Environment.NewLine +
                "    "+Environment.NewLine +
                "                 /  None              P0                b"+Environment.NewLine +
                "       b        <    0             R, R, P1             b"+Environment.NewLine +
                "                 \\   1             R, R, P0             b"+Environment.NewLine
                ,
                "<pre><code>m-config.      symbol         operations     final m-config.\n" +
                "\n" +
                "             /  None              P0                b\n" +
                "   b        &lt;    0             R, R, P1             b\n" +
                "             \\   1             R, R, P0             b\n" +
                "</code></pre>\n\n");
        }
    }
}