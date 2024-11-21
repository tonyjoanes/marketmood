namespace api.Application.ProductReview.DTOs
{
    public class CreateReviewRequest
    {
        public string ProductId { get; set; }
        public string ReviewId { get; set; }
        public int Rating { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public DateTime Date { get; set; }
        public bool VerifiedPurchase { get; set; }
        public int HelpfulVotes { get; set; }
    }
}
