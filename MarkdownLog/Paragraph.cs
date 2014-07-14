using System;
using System.Linq;

namespace MarkdownLog
{
    public class Paragraph : MarkdownElement
    {
        private readonly string _text;
        private int _wordWrapColumn;

        public Paragraph(string format, params object[] args) : this(string.Format(format, args))
        {
        }

        public Paragraph(string text)
        {
            _text = text;
            WordWrap = true;
            WordWrapColumn = 80;
        }

        public bool WordWrap { get; set; }

        public int WordWrapColumn
        {
            get { return _wordWrapColumn; }
            set { _wordWrapColumn = Math.Max(0, value); }
        }

        public override string ToMarkdown()
        {
            var originalLines = _text.SplitByLine();
            var linesWithoutFinalEmptyLine = (originalLines.Last() == "")
                ? originalLines.Take(originalLines.Count - 1)
                : originalLines;

            var lines = linesWithoutFinalEmptyLine.Select(line =>
            {
                var escapedLine = line.EscapeMarkdownCharacters();
                var wrapped = WordWrap ? escapedLine.WrapAt(WordWrapColumn) : escapedLine;
                return string.Join(Environment.NewLine, wrapped.SplitByLine());
            });

            return string.Join("  " + Environment.NewLine, lines) + Environment.NewLine;
        }
    }
}
