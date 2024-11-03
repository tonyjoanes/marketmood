using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace api.Persistence.Entities
{
    public class ScrapingErrorEntity
    {
        [BsonElement("Timestamp")]
        public DateTime Timestamp { get; set; }

        [BsonElement("ErrorMessage")]
        public string ErrorMessage { get; set; }

        [BsonElement("ErrorType")]
        public string ErrorType { get; set; }
    }
}