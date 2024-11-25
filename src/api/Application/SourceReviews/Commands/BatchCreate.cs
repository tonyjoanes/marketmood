using MediatR;
using api.Domain;
using api.Application.Reviews.DTOs;
using api.Persistence.Repositories;

namespace api.Application.SourceReviews.Commands
{
    public class BatchCreate
    {
        public class Command : IRequest<BatchReviewResult>
        {
            public List<CreateSourceReviewRequest> Reviews { get; set; }
        }

        public class Handler : IRequestHandler<Command, BatchReviewResult>
        {
            private readonly ISourceReviewRepository _repository;
            private readonly ILogger<Handler> _logger;

            public Handler(ISourceReviewRepository repository, ILogger<Handler> logger)
            {
                _repository = repository;
                _logger = logger;
            }

            public async Task<BatchReviewResult> Handle(Command request, CancellationToken cancellationToken)
            {
                var result = new BatchReviewResult
                {
                    Attempted = request.Reviews.Count
                };

                foreach (var reviewRequest in request.Reviews)
                {
                    try
                    {
                        var review = SourceReview.Create(
                            productId: reviewRequest.ProductId,
                            reviewId: reviewRequest.ReviewId,
                            rating: reviewRequest.Rating,
                            title: reviewRequest.Title,
                            content: reviewRequest.Content,
                            author: reviewRequest.Author,
                            date: reviewRequest.Date,
                            verifiedPurchase: reviewRequest.VerifiedPurchase,
                            helpfulVotes: reviewRequest.HelpfulVotes,
                            source: reviewRequest.Source
                        );

                        await _repository.CreateReview(review);
                        result.Inserted++;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to create review {ReviewId}", reviewRequest.ReviewId);
                        result.Duplicates++;
                    }
                }

                return result;
            }
        }
    }
}
