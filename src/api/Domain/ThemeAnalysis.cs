namespace api.Domain
{
    public class ThemeAnalysis
    {
        public string Theme { get; private set; }
        public int Count { get; private set; }
        public double SentimentScore { get; private set; }

        private ThemeAnalysis()
        {
            Theme = string.Empty;
        }

        public static ThemeAnalysis Create(string theme, int count, double sentimentScore)
        {
            return new ThemeAnalysis
            {
                Theme = theme,
                Count = count,
                SentimentScore = sentimentScore
            };
        }
    }
}