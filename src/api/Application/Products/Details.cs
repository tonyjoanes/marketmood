using api.Domain;
using api.Persistence;
using MediatR;
using MongoDB.Driver;

namespace api.Application.Products
{
    public static class Details
    {
        public class Query : IRequest<Product>
        {
            public string Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Product>
        {
            private readonly ProductRepository _productRepository;

            public Handler(ProductRepository productRepository)
            {
                _productRepository = productRepository;
            }

            public async Task<Product> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _productRepository.GetByIdAsync(request.Id);
            }
        }
    }
}