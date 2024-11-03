namespace api.Domain
{
    public class Review
    {
        public string ReviewerName { get; private set; }
        public string ReviewText { get; private set; }
        public decimal Rating { get; private set; }
        public DateTime ReviewDate { get; private set; }
        public ReviewAnalysis? Analysis { get; private set; }

        private Review()
        {
            ReviewerName = string.Empty;
            ReviewText = string.Empty;
        }

        public static Review Create(string reviewerName, string reviewText, decimal rating)
        {
            return new Review
            {
                ReviewerName = reviewerName,
                ReviewText = reviewText,
                Rating = rating,
                ReviewDate = DateTime.UtcNow
            };
        }

        public void AddAnalysis(ReviewAnalysis analysis)
        {
            Analysis = analysis;
        }
    }
}