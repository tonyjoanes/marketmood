namespace api.Application.Reviews.DTOs
{
    public class ReviewAnalysisRequest
    {
        public double SentimentScore { get; set; }
        public List<string> KeyPhrases { get; set; }
        public List<ThemeAnalysisDto> Themes { get; set; }
    }
}
