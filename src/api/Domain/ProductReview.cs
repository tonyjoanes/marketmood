namespace api.Domain
{
    public class ProductReview
    {
        public string? Id { get; private set; }
        public string ReviewId { get; private set; }  // References the original review
        public int Rating { get; private set; }
        public string Content { get; private set; }
        public DateTime ReviewDate { get; private set; }
        public double SentimentScore { get; private set; }
        public List<string> KeyPhrases { get; private set; }
        public List<ThemeAnalysis> Themes { get; private set; }
        public DateTime AnalyzedAt { get; private set; }

        private ProductReview()
        {
            ReviewId = string.Empty;
            Content = string.Empty;
            KeyPhrases = new List<string>();
            Themes = new List<ThemeAnalysis>();
        }

        public static ProductReview CreateFromReview(SourceReview review, double sentimentScore,
            List<string> keyPhrases, List<ThemeAnalysis> themes)
        {
            return new ProductReview
            {
                ReviewId = review.ReviewId,
                Rating = review.Rating,
                Content = review.Content,
                ReviewDate = review.Date,
                SentimentScore = sentimentScore,
                KeyPhrases = keyPhrases,
                Themes = themes,
                AnalyzedAt = DateTime.UtcNow
            };
        }

        public void SetId(string id)
        {
            if (!string.IsNullOrEmpty(Id))
                throw new InvalidOperationException("Cannot change existing ID");
            Id = id;
        }
    }
}
