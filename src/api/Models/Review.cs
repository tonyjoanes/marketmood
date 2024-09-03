using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace api.Models
{
    public class Review
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("ProductId")]
        public string ProductId { get; set; }

        [BsonElement("ReviewId")]
        public string ReviewId { get; set; }

        [BsonElement("Text")]
        public string Text { get; set; }

        [BsonElement("Rating")]
        public string Rating { get; set; }

        [BsonElement("CaptureDate")]
        public DateTime CaptureDate { get; set; }

        [BsonElement("Sentiment")]
        public string Sentiment { get; set; }
    }
}