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

    public class ScrapingErrorEntity
    {
        [BsonElement("Timestamp")]
        public DateTime Timestamp { get; set; }

        [BsonElement("ErrorMessage")]
        public string ErrorMessage { get; set; }

        [BsonElement("ErrorType")]
        public string ErrorType { get; set; }
    }

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

    public class ReviewAnalysisEntity
    {
        [BsonElement("SentimentScore")]
        public double SentimentScore { get; set; }

        [BsonElement("SentimentMagnitude")]
        public double SentimentMagnitude { get; set; }

        [BsonElement("Keywords")]
        public List<KeywordEntity> Keywords { get; set; }

        [BsonElement("Themes")]
        public List<string> Themes { get; set; }

        [BsonElement("ProcessedAt")]
        public DateTime ProcessedAt { get; set; }
    }

    public class KeywordEntity
    {
        [BsonElement("Word")]
        public string Word { get; set; }

        [BsonElement("Weight")]
        public double Weight { get; set; }
    }

    public class ProductAnalysisEntity
    {
        [BsonElement("OverallSentiment")]
        public double OverallSentiment { get; set; }

        [BsonElement("ReviewCount")]
        public int ReviewCount { get; set; }

        [BsonElement("AverageRating")]
        public decimal AverageRating { get; set; }

        [BsonElement("RatingDistribution")]
        public Dictionary<int, int> RatingDistribution { get; set; }

        [BsonElement("CommonThemes")]
        public List<ThemeAnalysisEntity> CommonThemes { get; set; }

        [BsonElement("KeyPhrases")]
        public List<KeyPhraseEntity> KeyPhrases { get; set; }

        [BsonElement("SentimentTrend")]
        public List<SentimentTrendPointEntity> SentimentTrend { get; set; }

        [BsonElement("LastUpdated")]
        public DateTime LastUpdated { get; set; }
    }

    public class ThemeAnalysisEntity
    {
        [BsonElement("Theme")]
        public string Theme { get; set; }

        [BsonElement("Count")]
        public int Count { get; set; }

        [BsonElement("SentimentScore")]
        public double SentimentScore { get; set; }

        [BsonElement("Keywords")]
        public List<string> Keywords { get; set; }
    }

    public class KeyPhraseEntity
    {
        [BsonElement("Phrase")]
        public string Phrase { get; set; }

        [BsonElement("Frequency")]
        public int Frequency { get; set; }

        [BsonElement("SentimentScore")]
        public double SentimentScore { get; set; }
    }

    public class SentimentTrendPointEntity
    {
        [BsonElement("Date")]
        public DateTime Date { get; set; }

        [BsonElement("Sentiment")]
        public double Sentiment { get; set; }

        [BsonElement("ReviewCount")]
        public int ReviewCount { get; set; }
    }
}