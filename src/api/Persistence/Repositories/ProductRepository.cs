using api.Domain;
using api.Persistence.Entities;
using MongoDB.Driver;

namespace api.Persistence.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMongoCollection<ProductEntity> _productsCollection;
        private readonly IMongoCollection<ProductAnalysisEntity> _productAnalysisCollection;
        private readonly ILogger<ProductRepository> _logger;

        public ProductRepository(IMongoDatabase database, ILogger<ProductRepository> logger)
        {
            _productsCollection = database.GetCollection<ProductEntity>("Products");
            _productAnalysisCollection = database.GetCollection<ProductAnalysisEntity>("ProductAnalyses");
            _logger = logger;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            var entities = await _productsCollection.Find(_ => true).ToListAsync();
            return entities.Select(MapToDomain).ToList();
        }

        public async Task<Product> GetByIdAsync(string id)
        {
            var entity = await _productsCollection.Find(p => p.Id == id).FirstOrDefaultAsync();
            return entity != null ? MapToDomain(entity) : null;
        }

        public async Task CreateAsync(Product product)
        {
            var entity = MapToEntity(product);
            await _productsCollection.InsertOneAsync(entity);
            product.SetId(entity.Id);
        }

        public async Task UpdateAsync(string id, Product product)
        {
            var entity = MapToEntity(product);
            entity.Id = id;
            await _productsCollection.ReplaceOneAsync(p => p.Id == id, entity);
        }

        public async Task DeleteAsync(string id)
        {
            await _productsCollection.DeleteOneAsync(p => p.Id == id);
            await _productAnalysisCollection.DeleteOneAsync(p => p.ProductId == id);
        }

        public async Task UpdateProductAnalysis(string productId, double overallSentiment, int totalReviews, List<ThemeAnalysis> themes)
        {
            var entity = new ProductAnalysisEntity
            {
                ProductId = productId,
                OverallSentiment = overallSentiment,
                TotalReviews = totalReviews,
                ThemeSentiments = themes.Select(t => new ThemeAnalysisEntity
                {
                    Theme = t.Theme,
                    Count = t.Count,
                    SentimentScore = t.SentimentScore
                }).ToList(),
                LastUpdated = DateTime.UtcNow
            };

            var filter = Builders<ProductAnalysisEntity>.Filter.Eq(p => p.ProductId, productId);
            var options = new ReplaceOptions { IsUpsert = true };
            await _productAnalysisCollection.ReplaceOneAsync(filter, entity, options);
        }

        private static Product MapToDomain(ProductEntity entity)
        {
            var product = Product.Create(
                brand: entity.Brand,
                model: entity.Model,
                type: entity.Type,
                reviewCount: entity.ReviewCount,
                sentiment: entity.Sentiment,
                imageUrl: entity.ImageUrl ?? "/images/products/placeholder.jpg"
            );

            product.SetId(entity.Id);
            return product;
        }

        private static ProductEntity MapToEntity(Product domain)
        {
            if (domain == null)
                throw new ArgumentNullException(nameof(domain));

            return new ProductEntity
            {
                Id = domain.Id,
                Brand = domain.Brand ?? throw new ArgumentException("Name cannot be null"),
                Model = domain.Model ?? string.Empty,
                Type = domain.Type,
                ReviewCount = domain.ReviewCount,
                Sentiment = domain.Sentiment,
                ImageUrl = domain.ImageUrl,
                CreatedAt = domain.CreatedAt,
                UpdatedAt = domain.UpdatedAt,
                LastScrapedAt = domain.LastScrapedAt
            };
        }
    }
}
