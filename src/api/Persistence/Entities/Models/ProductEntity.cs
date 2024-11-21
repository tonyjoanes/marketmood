using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace api.Persistence.Entities
{
    public class ProductEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Model")]
        public string Model { get; set; }

        [BsonElement("Brand")]
        public string Brand { get; set; }

        [BsonElement("ImageUrl")]
        public string? ImageUrl { get; set; }

        [BsonElement("Type")]
        public string Type { get; set; }

        [BsonElement("ReviewCount")]
        public int ReviewCount { get; set; }

        [BsonElement("Sentiment")]
        public double Sentiment { get; set; }

        [BsonElement("CreatedAt")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("UpdatedAt")]
        public DateTime UpdatedAt { get; set; }

        [BsonElement("LastScrapedAt")]
        public DateTime? LastScrapedAt { get; set; }
    }
}
