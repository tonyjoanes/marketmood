namespace api.Application.Reviews.DTOs
{
    public class BatchReviewResult
    {
        public int Attempted { get; set; }
        public int Inserted { get; set; }
        public int Duplicates { get; set; }
    }
}