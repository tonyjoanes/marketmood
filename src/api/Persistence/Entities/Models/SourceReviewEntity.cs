using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using api.Domain;

public class SourceReviewEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string ProductId { get; set; }

    public int Rating { get; set; }

    public string Content { get; set; }

    public string Source { get; set; }

    public DateTime Date { get; set; }

    public ReviewStatus Status { get; set; }

    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; }
}
