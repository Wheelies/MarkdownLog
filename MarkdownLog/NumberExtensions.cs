using System;

namespace MarkdownLog
{
    public static class NumberExtensions
    {
        public static string ToColumnTitle(this int columnIndex)
        {
            int dividend = columnIndex + 1;
            string columnName = String.Empty;

            while (dividend > 0)
            {
                int modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo) + columnName;
                dividend = (dividend - modulo) / 26;
            }

            return columnName;
        }
    }
}