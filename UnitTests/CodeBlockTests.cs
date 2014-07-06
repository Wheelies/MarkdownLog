using MarkdownLog;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
                "    m-config.      symbol         operations     final m-config.\r\n" +
                "    \r\n" +
                "                 /  None              P0                b\r\n" +
                "       b        <    0             R, R, P1             b\r\n" +
                "                 \\   1             R, R, P0             b\r\n"
                ,
                "<pre><code>m-config.      symbol         operations     final m-config.\n" +
                "\n" +
                "             /  None              P0                b\n" +
                "   b        &lt;    0             R, R, P1             b\n" +
                "             \\   1             R, R, P0             b\n" +
                "</code></pre>\n");
        }
    }
}