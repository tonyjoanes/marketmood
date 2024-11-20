using api.Persistence.Entities.Models;
using api.Domain;
using MongoDB.Driver;

public class ProductReviewRepository : IProductReviewRepository
{
    private readonly IMongoCollection<ProductReviewEntity> _reviewsCollection;

    public ProductReviewRepository(IMongoDatabase database)
    {
        _reviewsCollection = database.GetCollection<ProductReviewEntity>("ProductReviews");
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

    public async Task DeleteAsync(string id)
    {
        await _reviewsCollection.DeleteOneAsync(r => r.Id == id);
    }

    public async Task<bool> ExistsAsync(string reviewId)
    {
        return await _reviewsCollection.Find(r => r.ReviewId == reviewId).AnyAsync();
    }

    private static ProductReview MapToDomain(ProductReviewEntity entity)
    {
        var review = ProductReview.Create(
            productId: entity.ProductId,
            reviewId: entity.ReviewId,
            rating: entity.Rating,
            title: entity.Title,
            content: entity.Content,
            author: entity.Author,
            date: entity.Date,
            verifiedPurchase: entity.VerifiedPurchase,
            helpfulVotes: entity.HelpfulVotes
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
            ReviewId = domain.ReviewId,
            Rating = domain.Rating,
            Title = domain.Title,
            Content = domain.Content,
            Author = domain.Author,
            Date = domain.Date,
            VerifiedPurchase = domain.VerifiedPurchase,
            HelpfulVotes = domain.HelpfulVotes,
            CreatedAt = domain.CreatedAt,
            UpdatedAt = domain.UpdatedAt
        };
    }
}
