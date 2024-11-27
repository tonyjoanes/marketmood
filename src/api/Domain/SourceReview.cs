namespace api.Domain
{
    public class SourceReview
    {
        public string? Id { get; private set; }
        public string ProductId { get; private set; }
        public int Rating { get; private set; }
        public string Content { get; private set; }
        public string Source { get; private set; }
        public DateTime Date { get; private set; }
        public ReviewStatus Status { get; private set; } = ReviewStatus.New;
        public DateTime CreatedAt { get; private set; }

        private SourceReview()
        {
            ProductId = string.Empty;
            Content = string.Empty;
        }

        public static SourceReview Create(string productId, int rating, string content, string source, DateTime date)
        {
            return new SourceReview
            {
                ProductId = productId,
                Rating = rating,
                Content = content,
                Source = source,
                Date = date,
                CreatedAt = DateTime.UtcNow
            };
        }

        public void SetId(string id) => Id = id;
        public void SetStatus(ReviewStatus status) => Status = status;
    }
}
