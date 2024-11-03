using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace api.Persistence.Entities
{
    public class KeyPhraseEntity
    {
        [BsonElement("Phrase")]
        public string Phrase { get; set; }

        [BsonElement("Frequency")]
        public int Frequency { get; set; }

        [BsonElement("SentimentScore")]
        public double SentimentScore { get; set; }
    }
}