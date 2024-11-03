using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace api.Persistence.Entities
{
    public class ProductEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; }

        [BsonElement("Urls")]
        public List<ProductUrlEntity> Urls { get; set; }

        [BsonElement("Description")]
        public string Description { get; set; }

        [BsonElement("Brand")]
        public string Brand { get; set; }

        [BsonElement("Categories")]
        public List<string> Categories { get; set; }

        [BsonElement("Reviews")]
        public List<ReviewEntity> Reviews { get; set; }

        [BsonElement("Analysis")]
        public ProductAnalysisEntity Analysis { get; set; }

        [BsonElement("CreatedAt")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("UpdatedAt")]
        public DateTime UpdatedAt { get; set; }

        [BsonElement("LastScrapedAt")]
        public DateTime? LastScrapedAt { get; set; }
    }
}