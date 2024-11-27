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
                            rating: reviewRequest.Rating,
                            content: reviewRequest.Content,
                            source: reviewRequest.Source,
                            date: reviewRequest.Date
                        );

                        await _repository.CreateReview(review);
                        result.Inserted++;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to create review for product {ProductId}", reviewRequest.ProductId);
                        result.Duplicates++;
                    }
                }

                return result;
            }
        }
    }
}
