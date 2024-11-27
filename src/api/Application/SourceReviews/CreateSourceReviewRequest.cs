namespace api.Application.Reviews.DTOs
{
    public class CreateSourceReviewRequest
    {
        public string ProductId { get; set; }
        public int Rating { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public DateTime Date { get; set; }
        public bool VerifiedPurchase { get; set; }
        public int HelpfulVotes { get; set; }
        public string Source { get; set; }  // e.g., "Amazon", "Argos"
    }
}
