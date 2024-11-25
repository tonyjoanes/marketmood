using api.Persistence.Repositories;
using MediatR;

namespace api.Application.ProductReviews.Commands
{
    public class Delete
    {
        public class Command : IRequest<Unit>
        {
            public string Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IProductReviewRepository _repository;

            public Handler(IProductReviewRepository repository)
            {
                _repository = repository;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var review = await _repository.GetByIdAsync(request.Id);

                if (review == null)
                    throw new Exception("ProductReview not found");

                await _repository.DeleteAsync(request.Id);
                return Unit.Value;
            }
        }
    }
}
