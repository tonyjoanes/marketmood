using api.Domain;
using api.Persistence;
using MediatR;
using MongoDB.Driver;

namespace api.Application.Products
{
    public static class Delete
    {
        public class Command : IRequest
        {
            public string Id { get; set; }
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

                await _productRepository.DeleteAsync(request.Id);
            }
        }
    }
}