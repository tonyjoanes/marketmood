using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace api.Persistence.Entities
{
    public class ProductUrlEntity
    {
        [BsonElement("Domain")]
        public string Domain { get; set; }

        [BsonElement("Url")]
        public string Url { get; set; }

        [BsonElement("ProductId")]
        public string ProductId { get; set; }

        [BsonElement("LastScrapedAt")]
        public DateTime? LastScrapedAt { get; set; }

        [BsonElement("Active")]
        public bool Active { get; set; } = true;

        [BsonElement("ScrapingErrors")]
        public List<ScrapingErrorEntity> ScrapingErrors { get; set; }
    }
}