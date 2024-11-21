using api.Application.Common.Exceptions;
using MediatR;

namespace api.Application.ProductReview.Commands
{
    public class Create
    {
        public class Command : IRequest<Domain.ProductReview>
        {
            public string ProductId { get; set; }
            public string ReviewId { get; set; }
            public int Rating { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }
            public string Author { get; set; }
            public DateTime Date { get; set; }
            public bool VerifiedPurchase { get; set; }
            public int HelpfulVotes { get; set; }
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
                if (await _repository.ExistsAsync(request.ReviewId))
                    throw new DuplicateReviewException();

                var review = Domain.ProductReview.Create(
                    request.ProductId,
                    request.ReviewId,
                    request.Rating,
                    request.Title,
                    request.Content,
                    request.Author,
                    request.Date,
                    request.VerifiedPurchase,
                    request.HelpfulVotes
                );

                return await _repository.AddAsync(review);
            }
        }
    }
}
