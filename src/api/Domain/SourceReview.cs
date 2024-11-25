namespace api.Domain
{
    public class SourceReview
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
        public DateTime CreatedAt { get; private set; }
        public ReviewStatus Status { get; private set; }
        public string Source { get; private set; }  // e.g., "Amazon", "Argos"

        private SourceReview()
        {
            ProductId = string.Empty;
            ReviewId = string.Empty;
            Title = string.Empty;
            Content = string.Empty;
            Author = string.Empty;
            Source = string.Empty;
            CreatedAt = DateTime.UtcNow;
            Status = ReviewStatus.New;
        }

        private SourceReview(string productId, string reviewId, int rating, string title,
            string content, string author, DateTime date, bool verifiedPurchase,
            int helpfulVotes, string source)
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
            Source = source;
            CreatedAt = DateTime.UtcNow;
            Status = ReviewStatus.New;
        }

        public static SourceReview Create(
            string productId,
            string reviewId,
            int rating,
            string title,
            string content,
            string author,
            DateTime date,
            bool verifiedPurchase,
            int helpfulVotes,
            string source)
        {
            return new SourceReview(
                productId, reviewId, rating, title, content,
                author, date, verifiedPurchase, helpfulVotes, source);
        }

        public void MarkAsProcessing()
        {
            Status = ReviewStatus.Processing;
        }

        public void MarkAsProcessed()
        {
            Status = ReviewStatus.Analyzed;
        }

        public void MarkAsFailed()
        {
            Status = ReviewStatus.Failed;
        }

        public void SetId(string id)
        {
            if (!string.IsNullOrEmpty(Id))
                throw new InvalidOperationException("Cannot change existing ID");
            Id = id;
        }
    }
}
