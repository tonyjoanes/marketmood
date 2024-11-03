using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace api.Persistence.Entities
{
    public class SentimentTrendPointEntity
    {
        [BsonElement("Date")]
        public DateTime Date { get; set; }

        [BsonElement("Sentiment")]
        public double Sentiment { get; set; }

        [BsonElement("ReviewCount")]
        public int ReviewCount { get; set; }
    }
}