using api.Persistence.Repositories;
using MediatR;

namespace api.Application.ProductReview.Queries

{
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
