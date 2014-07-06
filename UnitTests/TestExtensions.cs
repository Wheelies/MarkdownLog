using System.Diagnostics;
using System.Text;
using MarkdownLog;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.MarkdownLog
{
    public static class TestExtensions
    {
        public static void WriteToTrace(this IMarkdownElement element)
        {
            var markdown = element.ToMarkdown();

            markdown.WriteToTraceWithDelimiter("Markdown");

            Trace.WriteLine("");
            
            var markdownSharp = new MarkdownToHtmlConverter();
            var html = markdownSharp.Transform(markdown);

            html.WriteToTraceWithDelimiter("HTML");
        }

        public static void AssertOutputEquals(this IMarkdownElement element, string expectedMarkdown, string expectedHtml = null)
        {
            var markdown = element.ToMarkdown();

            markdown.WriteToTraceWithDelimiter("Markdown");
            
            Trace.WriteLine("");

            var markdownSharp = new MarkdownToHtmlConverter();
            var html = markdownSharp.Transform(markdown);

            html.WriteToTraceWithDelimiter("HTML");

            Trace.WriteLine("");

            if (expectedMarkdown != markdown)
                Assert.Fail("Unexpected Markdown:\r\n\r\n{0}", BuildOutputWithDelimiter(expectedMarkdown, "Expected Markdown"));
            else
                Trace.WriteLine("Markdown output meets expectations");

            if (expectedHtml != null)
            {
                if (expectedHtml != html)
                    Assert.Fail("Unexpected HTML:\r\n\r\n{0}", BuildOutputWithDelimiter(html, "Expected HTML"));
                else
                    Trace.WriteLine("HTML output meets expectations");
            }
        }

        private static void WriteToTraceWithDelimiter(this string output, string type)
        {
            var builder = BuildOutputWithDelimiter(output, type);
            Trace.Write(builder.ToString());
        }

        private static StringBuilder BuildOutputWithDelimiter(string output, string type)
        {
            var builder = new StringBuilder();
            builder.AppendFormat("---BEGIN {0}---\r\n", type);
            builder.Append(output);
            builder.AppendFormat("---END {0}---\r\n", type);
            return builder;
        }
    }
}