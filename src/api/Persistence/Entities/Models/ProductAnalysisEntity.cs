using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace api.Persistence.Entities
{
    public class ProductAnalysisEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string ProductId { get; set; }
        public double OverallSentiment { get; set; }
        public int TotalReviews { get; set; }
        public List<ThemeAnalysisEntity> ThemeSentiments { get; set; } = new();
        public DateTime LastUpdated { get; set; }
    }
}
