namespace api.Domain
{
    public class ProductReview
    {
        public string? Id { get; private set; }
        public string ProductId { get; private set; }
        public string ReviewId { get; private set; }
        public int Rating { get; private set; }
        public string Title { get; private set; }
        public string Content { get; private set; }
        public string Author { get; private set; }
        public DateTime Date { get; private set; }
        public bool VerifiedPurchase { get; private set; }
        public int HelpfulVotes { get; private set; }
        public ReviewAnalysis Analysis { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        private ProductReview()
        {
            ProductId = string.Empty;
            ReviewId = string.Empty;
            Title = string.Empty;
            Content = string.Empty;
            Author = string.Empty;
            Analysis = new ReviewAnalysis();
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        private ProductReview(string productId, string reviewId, int rating, string title, string content,
            string author, DateTime date, bool verifiedPurchase, int helpfulVotes)
        {
            if (string.IsNullOrWhiteSpace(productId))
                throw new ArgumentException("ProductId cannot be empty");
            if (string.IsNullOrWhiteSpace(reviewId))
                throw new ArgumentException("ReviewId cannot be empty");
            if (rating < 1 || rating > 5)
                throw new ArgumentException("Rating must be between 1 and 5");

            ProductId = productId;
            ReviewId = reviewId;
            Rating = rating;
            Title = title;
            Content = content;
            Author = author;
            Date = date;
            VerifiedPurchase = verifiedPurchase;
            HelpfulVotes = helpfulVotes;
            Analysis = new ReviewAnalysis();
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public static ProductReview Create(string productId, string reviewId, int rating, string title,
            string content, string author, DateTime date, bool verifiedPurchase, int helpfulVotes)
        {
            return new ProductReview(productId, reviewId, rating, title, content, author, date, verifiedPurchase, helpfulVotes);
        }

        public void UpdateAnalysis(ReviewAnalysis analysis)
        {
            Analysis = analysis;
            UpdatedAt = DateTime.UtcNow;
        }

        internal void SetId(string id)
        {
            if (!string.IsNullOrEmpty(Id))
                throw new InvalidOperationException("Cannot change existing ID");
            Id = id;
        }
    }
}
