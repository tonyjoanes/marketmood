using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace api.Models
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; }

        [BsonElement("Urls")]
        public List<ProductUrl> Urls { get; set; }
    }

    public class ProductUrl
    {
        [BsonElement("Domain")]
        public string Domain { get; set; }

        [BsonElement("Url")]
        public string Url { get; set; }

        [BsonElement("ProductId")]
        public string ProductId { get; set; }
    }
}