namespace api.Application.ProductReview.DTOs
{
    public class BatchReviewResult
    {
        public int Attempted { get; set; }
        public int Inserted { get; set; }
        public int Duplicates { get; set; }
    }
}
