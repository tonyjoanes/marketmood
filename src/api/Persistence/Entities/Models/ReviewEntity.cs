using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace api.Persistence.Entities
{
    public class ReviewEntity
    {
        [BsonElement("ExternalId")]
        public string ExternalId { get; set; }

        [BsonElement("Source")]
        public string Source { get; set; }

        [BsonElement("ReviewText")]
        public string ReviewText { get; set; }

        [BsonElement("Rating")]
        public decimal Rating { get; set; }

        [BsonElement("ReviewerName")]
        public string ReviewerName { get; set; }

        [BsonElement("ReviewDate")]
        public DateTime ReviewDate { get; set; }

        [BsonElement("Verified")]
        public bool Verified { get; set; }

        [BsonElement("Analysis")]
        public ReviewAnalysisEntity Analysis { get; set; }

        [BsonElement("ScrapedAt")]
        public DateTime ScrapedAt { get; set; }

        [BsonElement("ProcessedAt")]
        public DateTime? ProcessedAt { get; set; }
    }
}