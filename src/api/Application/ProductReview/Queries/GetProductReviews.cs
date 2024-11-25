using MediatR;
using api.Persistence.Repositories;

namespace api.Application.ProductReviews.Queries
{
    public class GetProductReview
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

    public class ListByProduct
    {
        public class Query : IRequest<List<Domain.ProductReview>>
        {
            public string ProductId { get; set; }
        }

        public class Handler : IRequestHandler<Query, List<Domain.ProductReview>>
        {
            private readonly IProductReviewRepository _repository;

            public Handler(IProductReviewRepository repository)
            {
                _repository = repository;
            }

            public async Task<List<Domain.ProductReview>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _repository.GetReviewsByProductAsync(request.ProductId);
            }
        }
    }
}
