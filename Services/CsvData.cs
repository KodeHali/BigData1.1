namespace NewBlazorApp.Services
{
    public class CsvData
    {
        public List<string> Headers { get; set; } = new List<string>();
        public List<List<string>> Rows { get; set; } = new List<List<string>>();
    }
}
