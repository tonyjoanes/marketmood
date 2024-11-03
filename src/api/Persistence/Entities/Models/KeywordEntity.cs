using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace api.Persistence.Entities
{
    public class KeywordEntity
    {
        [BsonElement("Word")]
        public string Word { get; set; }

        [BsonElement("Weight")]
        public double Weight { get; set; }
    }
}