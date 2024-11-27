using api.Domain;

namespace api.Persistence.Repositories
{
    public interface ISourceReviewRepository
    {
        Task<IEnumerable<SourceReview>> GetReviewsByStatuses(ReviewStatus[] statuses);
        Task<SourceReview> GetReviewById(string id);
        Task CreateReview(SourceReview review);
        Task UpdateReview(SourceReview review);
    }
}
