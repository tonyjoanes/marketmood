using api.Domain;
using MongoDB.Driver;

namespace api.Persistence.Repositories
{
    public class SourceReviewRepository : ISourceReviewRepository
    {
        private readonly IMongoCollection<SourceReviewEntity> _reviewsCollection;
        private readonly ILogger<SourceReviewRepository> _logger;

        public SourceReviewRepository(IMongoDatabase database, ILogger<SourceReviewRepository> logger)
        {
            _reviewsCollection = database.GetCollection<SourceReviewEntity>("SourceReviews");
            _logger = logger;
        }

        public async Task<IEnumerable<SourceReview>> GetReviewsByStatuses(ReviewStatus[] statuses)
        {
            try
            {
                var filter = Builders<SourceReviewEntity>.Filter.In(r => r.Status, statuses);
                var entities = await _reviewsCollection.Find(filter)
                    .Sort(Builders<SourceReviewEntity>.Sort.Ascending(r => r.CreatedAt))
                    .ToListAsync();

                return entities.Select(MapReviewToDomain);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting reviews by statuses: {Statuses}", string.Join(", ", statuses));
                throw;
            }
        }

        public async Task<SourceReview> GetReviewById(string id)
        {
            try
            {
                var entity = await _reviewsCollection.Find(r => r.Id == id).FirstOrDefaultAsync();
                return entity != null ? MapReviewToDomain(entity) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting review: {ReviewId}", id);
                throw;
            }
        }

        public async Task CreateReview(SourceReview review)
        {
            try
            {
                var entity = MapReviewToEntity(review);
                await _reviewsCollection.InsertOneAsync(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating review for product: {ProductId}", review.ProductId);
                throw;
            }
        }

        public async Task UpdateReview(SourceReview review)
        {
            try
            {
                var entity = MapReviewToEntity(review);
                var result = await _reviewsCollection.ReplaceOneAsync(r => r.Id == review.Id, entity);
                if (!result.IsAcknowledged)
                    throw new Exception($"Failed to update review {review.Id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating review: {ReviewId}", review.Id);
                throw;
            }
        }

        private static SourceReview MapReviewToDomain(SourceReviewEntity entity)
        {
            var review = SourceReview.Create(
                productId: entity.ProductId,
                rating: entity.Rating,
                content: entity.Content,
                source: entity.Source,
                date: entity.Date
            );

            review.SetId(entity.Id);
            review.SetStatus(entity.Status);
            return review;
        }

        private static SourceReviewEntity MapReviewToEntity(SourceReview review)
        {
            return new SourceReviewEntity
            {
                ProductId = review.ProductId,
                Rating = review.Rating,
                Content = review.Content,
                Date = review.Date,
                Status = review.Status,
                CreatedAt = review.CreatedAt
            };
        }
    }
}
