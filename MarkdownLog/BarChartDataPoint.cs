namespace MarkdownLog
{
    public class BarChartDataPoint
    {
        private string _categoryName = "";

        public string CategoryName
        {
            get { return _categoryName; }
            set { _categoryName = value ?? ""; }
        }

        public double Value { get; set; }
    }
}