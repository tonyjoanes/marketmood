namespace api.Domain
{
    public class ProductAnalysis
    {
        public double OverallSentiment { get; private set; }
        public int ReviewCount { get; private set; }
        public Dictionary<int, int> RatingDistribution { get; private set; }
        public List<ThemeAnalysis> CommonThemes { get; private set; }
        public DateTime LastUpdated { get; private set; }

        public ProductAnalysis()
        {
            RatingDistribution = new Dictionary<int, int>();
            CommonThemes = new List<ThemeAnalysis>();
            LastUpdated = DateTime.UtcNow;
        }

        public void Update(double sentiment, int reviewCount,
            Dictionary<int, int> ratingDistribution, List<ThemeAnalysis> themes)
        {
            OverallSentiment = sentiment;
            ReviewCount = reviewCount;
            RatingDistribution = ratingDistribution;
            CommonThemes = themes;
            LastUpdated = DateTime.UtcNow;
        }
    }
}