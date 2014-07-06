namespace MarkdownLog
{
    public interface IIosTableViewCell
    {
        int RequiredWidth { get; }
        string BuildCodeFormattedString(int maximumWidth);
    }
}