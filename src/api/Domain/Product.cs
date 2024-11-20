namespace api.Domain
{
    public class Product
    {
        public string? Id { get; private set; }
        public string Brand { get; private set; }
        public string Model { get; private set; }
        public double Sentiment { get; private set; }
        public int ReviewCount { get; private set; }
        public string? ImageUrl { get; set; }
        public string Type { get; set; }
        public List<string> Categories { get; private set; }
        public List<ProductUrl> Urls { get; private set; }
        public List<ProductReview> Reviews { get; private set; }
        public ProductAnalysis Analysis { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public DateTime? LastScrapedAt { get; private set; }

        private Product()
        {
            Brand = string.Empty;
            Model = string.Empty;
            ReviewCount = 0;
            Categories = new List<string>();
            Urls = new List<ProductUrl>();
            Reviews = new List<ProductReview>();
            Analysis = new ProductAnalysis();
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        private Product(string brand, string model, string type, double sentiment, int reviewCount, string imageUrl)
        {
            if (string.IsNullOrWhiteSpace(brand))
                throw new ArgumentException("Brand cannot be empty");

            if (string.IsNullOrWhiteSpace(model))
                throw new ArgumentException("Model cannot be empty");

            Brand = brand;
            Model = model;
            Type = type;
            Sentiment = sentiment;
            ReviewCount = reviewCount;
            ImageUrl = imageUrl;
            Categories = [];
            Urls = new List<ProductUrl>();
            Reviews = new List<ProductReview>();
            Analysis = new ProductAnalysis();
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public static Product Create(string brand, string model, string type, double sentiment, int reviewCount, string imageUrl)
        {
            return new Product(brand, model, type, sentiment, reviewCount, imageUrl);
        }

        public void Update(string brand, string model, string type, double sentiment, int reviewCount, string imageUrl)
        {
            if (string.IsNullOrWhiteSpace(brand))
                throw new ArgumentException("Name cannot be empty");
            if (string.IsNullOrWhiteSpace(model))
                throw new ArgumentException("Model cannot be empty");

            Model = model;
            Brand = brand;
            Type = type;
            Sentiment = sentiment;
            ReviewCount = reviewCount;
            ImageUrl = imageUrl;
            UpdatedAt = DateTime.UtcNow;
        }

        public void AddUrl(string domain, string url)
        {
            var productUrl = new ProductUrl(domain, url);
            Urls.Add(productUrl);
            UpdatedAt = DateTime.UtcNow;
        }

        public void AddReview(ProductReview review)
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

        public void UpdateImageUrl(string newImagePath)
        {
            ImageUrl = newImagePath;
            UpdatedAt = DateTime.UtcNow;
        }

        internal void SetId(string id)
        {
            if (!string.IsNullOrEmpty(Id))
                throw new InvalidOperationException("Cannot change existing ID");

            Id = id;
        }
    }
}
