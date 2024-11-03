using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace api.Persistence.Entities
{
    public class ReviewAnalysisEntity
    {
        [BsonElement("SentimentScore")]
        public double SentimentScore { get; set; }

        [BsonElement("SentimentMagnitude")]
        public double SentimentMagnitude { get; set; }

        [BsonElement("Keywords")]
        public List<KeywordEntity> Keywords { get; set; }

        [BsonElement("Themes")]
        public List<string> Themes { get; set; }

        [BsonElement("ProcessedAt")]
        public DateTime ProcessedAt { get; set; }
    }
}