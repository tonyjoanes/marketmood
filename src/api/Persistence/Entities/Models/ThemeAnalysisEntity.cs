using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace api.Persistence.Entities
{
    public class ThemeAnalysisEntity
    {
        [BsonElement("Theme")]
        public string Theme { get; set; }

        [BsonElement("Count")]
        public int Count { get; set; }

        [BsonElement("SentimentScore")]
        public double SentimentScore { get; set; }

        [BsonElement("Keywords")]
        public List<string> Keywords { get; set; }
    }
}