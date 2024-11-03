using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace api.Persistence.Entities
{
    public class ProductAnalysisEntity
    {
        [BsonElement("OverallSentiment")]
        public double OverallSentiment { get; set; }

        [BsonElement("ReviewCount")]
        public int ReviewCount { get; set; }

        [BsonElement("AverageRating")]
        public decimal AverageRating { get; set; }

        [BsonElement("RatingDistribution")]
        public Dictionary<int, int> RatingDistribution { get; set; }

        [BsonElement("CommonThemes")]
        public List<ThemeAnalysisEntity> CommonThemes { get; set; }

        [BsonElement("KeyPhrases")]
        public List<KeyPhraseEntity> KeyPhrases { get; set; }

        [BsonElement("SentimentTrend")]
        public List<SentimentTrendPointEntity> SentimentTrend { get; set; }

        [BsonElement("LastUpdated")]
        public DateTime LastUpdated { get; set; }
    }
}