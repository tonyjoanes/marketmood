using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Domain
{
    public class Product
    {
        public string? Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Brand { get; private set; }
        public List<string> Categories { get; private set; }
        public List<ProductUrl> Urls { get; private set; }
        public List<Review> Reviews { get; private set; }
        public ProductAnalysis Analysis { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public DateTime? LastScrapedAt { get; private set; }

        private Product()
        {
            Name = string.Empty;
            Description = string.Empty;
            Brand = string.Empty;
            Categories = new List<string>();
            Urls = new List<ProductUrl>();
            Reviews = new List<Review>();
            Analysis = new ProductAnalysis();
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        private Product(string name, string description, string brand, List<string> categories)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Name cannot be empty");

            Name = name;
            Description = description;
            Brand = brand;
            Categories = categories ?? new List<string>();
            Urls = new List<ProductUrl>();
            Reviews = new List<Review>();
            Analysis = new ProductAnalysis();
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public static Product Create(string name, string description, string brand, List<string> categories)
        {
            return new Product(name, description, brand, categories);
        }

        public void Update(string name, string description, string brand, List<string> categories)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Name cannot be empty");

            Name = name;
            Description = description;
            Brand = brand;
            Categories = categories ?? new List<string>();
            UpdatedAt = DateTime.UtcNow;
        }

        public void AddUrl(string domain, string url)
        {
            var productUrl = new ProductUrl(domain, url);
            Urls.Add(productUrl);
            UpdatedAt = DateTime.UtcNow;
        }

        public void AddReview(Review review)
        {
            Reviews.Add(review);
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateAnalysis(ProductAnalysis analysis)
        {
            Analysis = analysis;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateLastScrapedAt(DateTime scrapedAt)
        {
            LastScrapedAt = scrapedAt;
            UpdatedAt = DateTime.UtcNow;
        }

        internal void SetId(string id)
        {
            if (!string.IsNullOrEmpty(Id))
                throw new InvalidOperationException("Cannot change existing ID");

            Id = id;
        }
    }

    // Supporting domain classes
    public class ProductUrl
    {
        public string Domain { get; private set; }
        public string Url { get; private set; }
        public DateTime? LastScrapedAt { get; private set; }

        private ProductUrl()
        {
            Domain = string.Empty;
            Url = string.Empty;
        }

        public ProductUrl(string domain, string url)
        {
            if (string.IsNullOrEmpty(domain))
                throw new ArgumentException("Domain cannot be empty");
            if (string.IsNullOrEmpty(url))
                throw new ArgumentException("URL cannot be empty");

            Domain = domain;
            Url = url;
        }

        public void UpdateLastScrapedAt(DateTime scrapedAt)
        {
            LastScrapedAt = scrapedAt;
        }
    }

    public class Review
    {
        public string ReviewerName { get; private set; }
        public string ReviewText { get; private set; }
        public decimal Rating { get; private set; }
        public DateTime ReviewDate { get; private set; }
        public ReviewAnalysis? Analysis { get; private set; }

        private Review()
        {
            ReviewerName = string.Empty;
            ReviewText = string.Empty;
        }

        public static Review Create(string reviewerName, string reviewText, decimal rating)
        {
            return new Review
            {
                ReviewerName = reviewerName,
                ReviewText = reviewText,
                Rating = rating,
                ReviewDate = DateTime.UtcNow
            };
        }

        public void AddAnalysis(ReviewAnalysis analysis)
        {
            Analysis = analysis;
        }
    }

    public class ProductAnalysis
    {
        public double OverallSentiment { get; private set; }
        public int ReviewCount { get; private set; }
        public Dictionary<int, int> RatingDistribution { get; private set; }
        public List<ThemeAnalysis> CommonThemes { get; private set; }
        public DateTime LastUpdated { get; private set; }

        public ProductAnalysis()
        {
            RatingDistribution = new Dictionary<int, int>();
            CommonThemes = new List<ThemeAnalysis>();
            LastUpdated = DateTime.UtcNow;
        }

        public void Update(double sentiment, int reviewCount,
            Dictionary<int, int> ratingDistribution, List<ThemeAnalysis> themes)
        {
            OverallSentiment = sentiment;
            ReviewCount = reviewCount;
            RatingDistribution = ratingDistribution;
            CommonThemes = themes;
            LastUpdated = DateTime.UtcNow;
        }
    }

    public class ThemeAnalysis
    {
        public string Theme { get; private set; }
        public int Count { get; private set; }
        public double SentimentScore { get; private set; }

        private ThemeAnalysis()
        {
            Theme = string.Empty;
        }

        public static ThemeAnalysis Create(string theme, int count, double sentimentScore)
        {
            return new ThemeAnalysis
            {
                Theme = theme,
                Count = count,
                SentimentScore = sentimentScore
            };
        }
    }

    public class ReviewAnalysis
    {
        public double SentimentScore { get; private set; }
        public double SentimentMagnitude { get; private set; }
        public List<string> Keywords { get; private set; }
        public DateTime ProcessedAt { get; private set; }

        public ReviewAnalysis()
        {
            Keywords = new List<string>();
            ProcessedAt = DateTime.UtcNow;
        }

        public void Update(double score, double magnitude, List<string> keywords)
        {
            SentimentScore = score;
            SentimentMagnitude = magnitude;
            Keywords = keywords;
            ProcessedAt = DateTime.UtcNow;
        }
    }
}