using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace api.Persistence.Entities
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using api.Domain;

    public class SourceReviewEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string ProductId { get; set; }
        public string RawText { get; set; }
        public ReviewStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastProcessingAttempt { get; set; }
        public ProductAnalysisEntity Analysis { get; set; }
        public int ProcessingAttempts { get; set; }
        public string Source { get; set; }
    }
}
