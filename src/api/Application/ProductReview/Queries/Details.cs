using api.Persistence.Repositories;
using MediatR;

namespace api.Application.ProductReview.Queries
{
    public class Details
    {
        public class Query : IRequest<Domain.ProductReview>
        {
            public string Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Domain.ProductReview>
        {
            private readonly IProductReviewRepository _repository;

            public Handler(IProductReviewRepository repository)
            {
                _repository = repository;
            }

            public async Task<Domain.ProductReview> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _repository.GetByIdAsync(request.Id);
            }
        }
    }
}
