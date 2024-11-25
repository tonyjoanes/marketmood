using api.Domain;

namespace api.Persistence.Repositories
{
    public interface ISourceReviewRepository
    {
        Task<IEnumerable<SourceReview>> GetReviewsByStatuses(ReviewStatus[] statuses);
        Task<SourceReview> GetReviewById(string id);
        Task UpdateReview(SourceReview review);
        Task<IEnumerable<SourceReview>> GetAnalyzedReviewsForProduct(string productId);
        Task CreateReview(SourceReview review);
    }
}
