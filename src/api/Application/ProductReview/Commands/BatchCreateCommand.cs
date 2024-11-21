using api.Application.Common.Exceptions;
using api.Application.ProductReview.DTOs;
using MediatR;

namespace api.Application.ProductReview.Commands
{

    public class BatchCreate
    {
        public class Command : IRequest<BatchReviewResult>
        {
            public List<CreateReviewRequest> Reviews { get; set; }
        }

        public class Handler : IRequestHandler<Command, BatchReviewResult>
        {
            private readonly IProductReviewRepository _repository;

            public Handler(IProductReviewRepository repository)
            {
                _repository = repository;
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
                        var review = Domain.ProductReview.Create
                        (
                            productId: reviewRequest.ProductId,
                            reviewId: reviewRequest.ReviewId,
                            rating: reviewRequest.Rating,
                            title: reviewRequest.Title,
                            content: reviewRequest.Content,
                            author: reviewRequest.Author,
                            date: reviewRequest.Date,
                            verifiedPurchase: reviewRequest.VerifiedPurchase,
                            helpfulVotes: reviewRequest.HelpfulVotes
                        );

                        await _repository.AddAsync(review);
                        result.Inserted++;
                    }
                    catch (DuplicateReviewException)
                    {
                        result.Duplicates++;
                    }
                }

                return result;
            }
        }
    }
}
