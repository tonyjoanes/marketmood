using api.Domain;
using api.Persistence;
using MediatR;
using MongoDB.Driver;

namespace api.Application.Products
{
    public static class Create
    {
        public class Command : IRequest<Product>
        {
            public string Name { get; set; }
            // Add other properties as needed
        }

        public class Handler : IRequestHandler<Command, Product>
        {
            private readonly ProductRepository _productRepository;

            public Handler(ProductRepository productRepository)
            {
                _productRepository = productRepository;
            }

            public async Task<Product> Handle(Command request, CancellationToken cancellationToken)
            {
                var product = Domain.Product.Create(request.Name);  // Use static factory method instead of constructor

                await _productRepository.CreateAsync(product);
                return product;
            }
        }
    }

}