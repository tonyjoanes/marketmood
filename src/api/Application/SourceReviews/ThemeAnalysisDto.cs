namespace api.Application.Reviews.DTOs
{
    public class ThemeAnalysisDto
    {
        public string Theme { get; set; }
        public int Count { get; set; }
        public double SentimentScore { get; set; }
        public List<string> SupportingPhrases { get; set; }
    }
}
