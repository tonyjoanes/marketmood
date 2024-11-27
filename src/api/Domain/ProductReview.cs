namespace api.Domain
{
    public class ProductReview
    {
        public string? Id { get; private set; }
        public string ProductId { get; private set; }
        public string SourceReviewId { get; private set; }
        public List<string> KeyPhrases { get; private set; }
        public DateTime ReviewDate { get; private set; }
        public double SentimentScore { get; private set; }
        public double AverageRating { get; private set; }
        public DateTime AnalyzedAt { get; private set; }
        public int ReviewCount { get; private set; }

        private ProductReview()
        {
            ProductId = string.Empty;
            SourceReviewId = string.Empty;
        }

        public static ProductReview Create(string productId, double averageRating, double sentimentScore, List<string> keyPhrases, int reviewCount)
        {
            return new ProductReview
            {
                ProductId = productId,
                AverageRating = averageRating,
                SentimentScore = sentimentScore,
                KeyPhrases = keyPhrases,
                AnalyzedAt = DateTime.UtcNow,
                ReviewCount = reviewCount
            };
        }

        public static ProductReview CreateFromSourceReview(
            SourceReview review,
            double sentimentScore)
        {
            return new ProductReview
            {
                ProductId = review.ProductId,
                SourceReviewId = review.Id!,
                ReviewDate = review.Date,
                SentimentScore = sentimentScore,
                AnalyzedAt = DateTime.UtcNow
            };
        }

        public void SetId(string id) => Id = id;
    }
}
