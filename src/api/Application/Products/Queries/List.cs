using api.Domain;
using api.Persistence.Repositories;
using MediatR;

namespace api.Application.Products.Queries
{
    public class List
    {
        public class Query : IRequest<List<Product>>
        {
        }

        public class Handler : IRequestHandler<Query, List<Product>>
        {
            private ProductRepository _productRepository;

            public Handler(ProductRepository productRepository)
            {
                _productRepository = productRepository;
            }

            public async Task<List<Product>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _productRepository.GetAllAsync();
            }
        }
    }
}