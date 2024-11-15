using api.Domain;
using api.Persistence.Entities;
using MongoDB.Driver;

namespace api.Persistence.Repositories
{
    public class ProductRepository
    {
        private readonly IMongoCollection<ProductEntity> _productsCollection;

        public ProductRepository(IMongoDatabase database)
        {
            _productsCollection = database.GetCollection<ProductEntity>("Products");
        }

        public async Task<List<Domain.Product>> GetAllAsync()
        {
            var entities = await _productsCollection.Find(_ => true).ToListAsync();
            return entities.Select(MapToDomain).ToList();
        }

        public async Task<Domain.Product> GetByIdAsync(string id)
        {
            var entity = await _productsCollection.Find(p => p.Id == id).FirstOrDefaultAsync();
            return entity != null ? MapToDomain(entity) : null;
        }

        public async Task CreateAsync(Domain.Product product)
        {
            var entity = MapToEntity(product);
            await _productsCollection.InsertOneAsync(entity);
            // Update the ID in the domain object after creation
            product.SetId(entity.Id);
        }

        public async Task UpdateAsync(string id, Domain.Product product)
        {
            var entity = MapToEntity(product);
            entity.Id = id;
            await _productsCollection.ReplaceOneAsync(p => p.Id == id, entity);
        }

        public async Task DeleteAsync(string id)
        {
            await _productsCollection.DeleteOneAsync(p => p.Id == id);
        }

        private static Product MapToDomain(ProductEntity entity)
        {
            var product = Product.Create(
                name: entity.Name,
                description: entity.Description ?? string.Empty,  // Provide default if null
                brand: entity.Brand ?? string.Empty,             // Provide default if null
                categories: entity.Categories ?? new List<string>()  // Provide empty list if null
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
                Id = domain.Id,  // Can be null for new products
                Name = domain.Name ?? throw new ArgumentException("Name cannot be null"),
                Description = domain.Description ?? string.Empty,
                Brand = domain.Brand ?? string.Empty,
                Categories = domain.Categories ?? new List<string>(),

                // Handle URLs
                Urls = (domain.Urls ?? new List<ProductUrl>()).Select(u => new ProductUrlEntity
                {
                    Domain = u.Domain ?? throw new ArgumentException("URL Domain cannot be null"),
                    Url = u.Url ?? throw new ArgumentException("URL cannot be null"),
                    LastScrapedAt = u.LastScrapedAt  // DateTime? can be null
                }).ToList(),

                // Handle Reviews
                Reviews = (domain.Reviews ?? new List<Review>()).Select(r => new ReviewEntity
                {
                    ReviewerName = r.ReviewerName ?? throw new ArgumentException("Reviewer name cannot be null"),
                    ReviewText = r.ReviewText ?? string.Empty,
                    Rating = r.Rating,
                    ReviewDate = r.ReviewDate,
                    Analysis = r.Analysis == null ? null : new ReviewAnalysisEntity
                    {
                        SentimentScore = r.Analysis.SentimentScore,
                        SentimentMagnitude = r.Analysis.SentimentMagnitude,
                        Keywords = (r.Analysis.Keywords ?? new List<string>())
                            .Select(k => new KeywordEntity
                            {
                                Word = k ?? throw new ArgumentException("Keyword cannot be null")
                            }).ToList(),
                        ProcessedAt = r.Analysis.ProcessedAt
                    }
                }).ToList(),

                // Handle Analysis
                Analysis = domain.Analysis == null ? null : new ProductAnalysisEntity
                {
                    OverallSentiment = domain.Analysis.OverallSentiment,
                    ReviewCount = domain.Analysis.ReviewCount,
                    RatingDistribution = domain.Analysis.RatingDistribution ??
                        new Dictionary<int, int>(),
                    CommonThemes = (domain.Analysis.CommonThemes ?? new List<ThemeAnalysis>())
                        .Select(t => new ThemeAnalysisEntity
                        {
                            Theme = t.Theme ?? throw new ArgumentException("Theme cannot be null"),
                            Count = t.Count,
                            SentimentScore = t.SentimentScore
                        }).ToList(),
                    LastUpdated = domain.Analysis.LastUpdated
                },

                CreatedAt = domain.CreatedAt,
                UpdatedAt = domain.UpdatedAt,
                LastScrapedAt = domain.LastScrapedAt  // DateTime? can be null
            };
        }
    }
}