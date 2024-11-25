using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace api.Persistence.Entities
{
    public class ThemeAnalysisEntity
    {
        public string Theme { get; set; }
        public int Count { get; set; }
        public double SentimentScore { get; set; }
    }
}
