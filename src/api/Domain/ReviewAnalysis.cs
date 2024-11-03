namespace api.Domain
{
    public class ReviewAnalysis
    {
        public double SentimentScore { get; private set; }
        public double SentimentMagnitude { get; private set; }
        public List<string> Keywords { get; private set; }
        public DateTime ProcessedAt { get; private set; }

        public ReviewAnalysis()
        {
            Keywords = new List<string>();
            ProcessedAt = DateTime.UtcNow;
        }

        public void Update(double score, double magnitude, List<string> keywords)
        {
            SentimentScore = score;
            SentimentMagnitude = magnitude;
            Keywords = keywords;
            ProcessedAt = DateTime.UtcNow;
        }
    }
}