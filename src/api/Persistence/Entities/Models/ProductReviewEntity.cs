using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace api.Persistence.Entities.Models
{
    public class ProductReviewEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string ProductId { get; set; }

        [BsonElement("review_id")]
        public string ReviewId { get; set; }

        public string Source { get; set; }

        public double SentimentScore { get; set; }
        public double AverageRating { get; set; }
        public List<string> KeyPhrases { get; set; }
        public DateTime AnalyzedAt { get; set; }
        public int ReviewCount { get; set; }

        public DateTime Date { get; set; }

        [BsonElement("created_at")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}
