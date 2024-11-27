using api.Domain;
using api.Persistence.Entities.Models;
using MongoDB.Driver;

namespace api.Persistence.Repositories
{
    public class ProductReviewRepository : IProductReviewRepository
    {
        private readonly IMongoCollection<ProductReviewEntity> _reviewsCollection;
        private readonly ILogger<ProductReviewRepository> _logger;

        public ProductReviewRepository(IMongoDatabase database, ILogger<ProductReviewRepository> logger)
        {
            _reviewsCollection = database.GetCollection<ProductReviewEntity>("ProductReviews");
            _logger = logger;
        }

        public async Task<ProductReview> GetByIdAsync(string id)
        {
            var entity = await _reviewsCollection.Find(p => p.Id == id).FirstOrDefaultAsync();
            return entity != null ? MapToDomain(entity) : null;
        }

        public async Task<List<ProductReview>> GetReviewsByProductAsync(string productId)
        {
            var entities = await _reviewsCollection.Find(r => r.ProductId == productId).ToListAsync();
            return entities.Select(MapToDomain).ToList();
        }

        public async Task<ProductReview> AddAsync(ProductReview review)
        {
            var entity = MapToEntity(review);
            await _reviewsCollection.InsertOneAsync(entity);
            review.SetId(entity.Id);
            return review;
        }

        private static ProductReview MapToDomain(ProductReviewEntity entity)
        {
            var review = ProductReview.Create(
                productId: entity.ProductId,
                averageRating: entity.AverageRating,
                sentimentScore: entity.SentimentScore,
                keyPhrases: entity.KeyPhrases, reviewCount: entity.ReviewCount
            );

            review.SetId(entity.Id);
            return review;
        }

        private static ProductReviewEntity MapToEntity(ProductReview domain)
        {
            return new ProductReviewEntity
            {
                Id = domain.Id,
                ProductId = domain.ProductId,
                Date = domain.ReviewDate,
                SentimentScore = domain.SentimentScore,
                AverageRating = domain.AverageRating
            };
        }

        public Task DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(string reviewId)
        {
            throw new NotImplementedException();
        }
    }
}
