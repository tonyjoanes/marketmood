using api.Domain;

public interface IProductReviewRepository
{
    Task<ProductReview> GetByIdAsync(string id);
    Task<List<ProductReview>> GetReviewsByProductAsync(string productId);
    Task<ProductReview> AddAsync(ProductReview review);
    Task DeleteAsync(string id);
    Task<bool> ExistsAsync(string reviewId);
}
