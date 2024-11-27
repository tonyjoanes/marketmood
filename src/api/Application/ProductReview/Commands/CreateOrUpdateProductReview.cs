using api.Persistence.Repositories;
using MediatR;

namespace api.Application.ProductReviews.Commands
{
    public class CreateOrUpdateProductReview
    {
        public record Command : IRequest<Domain.ProductReview>
        {
            public string ProductId { get; init; }
            public double AverageRating { get; init; }
            public double SentimentScore { get; init; }
            public List<string> KeyPhrases { get; init; }
            public int ReviewCount { get; init; }
        }

        public class Handler : IRequestHandler<Command, Domain.ProductReview>
        {
            private readonly IProductReviewRepository _repository;

            public Handler(IProductReviewRepository repository)
            {
                _repository = repository;
            }

            public async Task<Domain.ProductReview> Handle(Command request, CancellationToken cancellationToken)
            {
                var review = Domain.ProductReview.Create(
                    request.ProductId,
                    request.AverageRating,
                    request.SentimentScore,
                    request.KeyPhrases,
                    request.ReviewCount
                );

                return await _repository.AddAsync(review);
            }
        }
    }
}
