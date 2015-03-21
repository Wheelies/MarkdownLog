using System.Diagnostics;
using System.Text;
using MarkdownLog;
using NUnit.Framework;
using System;

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

            var html = markdown.MarkdownToHtml();

            html.WriteToTraceWithDelimiter("HTML");

            Trace.WriteLine("");

            if (expectedMarkdown != markdown)
                Assert.Fail("Unexpected Markdown:{0}{0}{1}", Environment.NewLine, BuildOutputWithDelimiter(expectedMarkdown, "Expected Markdown"));
            else
                Trace.WriteLine("Markdown output meets expectations");

            if (expectedHtml != null)
            {
                Assert.AreEqual(expectedHtml, html,
                    string.Format("Unexpected HTML:{0}{0}{1}", Environment.NewLine, BuildOutputWithDelimiter(expectedHtml, "Expected HTML")));
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
            builder.AppendFormat("---BEGIN {0}---"+Environment.NewLine, type);
            builder.Append(output);
            builder.AppendFormat("---END {0}---"+Environment.NewLine, type);
            return builder;
        }
    }
}