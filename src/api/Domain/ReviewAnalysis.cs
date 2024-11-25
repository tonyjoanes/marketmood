namespace api.Domain
{
    public class ReviewAnalysis
    {
        public double SentimentScore { get; private set; }
        public List<string> KeyPhrases { get; private set; }
        public List<ThemeAnalysis> Themes { get; private set; }
        public DateTime AnalyzedAt { get; private set; }

        private ReviewAnalysis()
        {
            KeyPhrases = new List<string>();
            Themes = new List<ThemeAnalysis>();
        }

        public static ReviewAnalysis Create(double sentimentScore, IEnumerable<string> keyPhrases, IEnumerable<ThemeAnalysis> themes)
        {
            return new ReviewAnalysis
            {
                SentimentScore = sentimentScore,
                KeyPhrases = keyPhrases.ToList(),
                Themes = themes.ToList(),
                AnalyzedAt = DateTime.UtcNow
            };
        }

        // Add this new method for creating empty analysis
        public static ReviewAnalysis CreateEmpty()
        {
            return new ReviewAnalysis
            {
                SentimentScore = 0,
                KeyPhrases = new List<string>(),
                Themes = new List<ThemeAnalysis>(),
                AnalyzedAt = DateTime.UtcNow
            };
        }
    }
}
