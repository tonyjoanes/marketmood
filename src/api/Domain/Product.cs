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
        public string? ImagePath { get; set; }

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

        public void UpdateImageUrl(string newImagePath)
        {
            ImagePath = newImagePath;
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