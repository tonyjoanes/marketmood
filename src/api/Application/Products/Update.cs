using api.Domain;
using api.Persistence;
using MediatR;
using MongoDB.Driver;

namespace api.Application.Products
{
    public static class Update
    {
        public class Command : IRequest
        {
            public string Id { get; set; }
            public string Name { get; set; }
            // Add other properties as needed
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly ProductRepository _productRepository;

            public Handler(ProductRepository productRepository)
            {
                _productRepository = productRepository;
            }

            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                var product = await _productRepository.GetByIdAsync(request.Id);

                if (product == null)
                    throw new Exception("Product not found");

                product.Update(request.Name);
                // Update other properties

                await _productRepository.UpdateAsync(request.Id, product);
            }
        }
    }
}