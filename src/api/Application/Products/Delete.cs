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
            private readonly ImageService _imageService;

            public Handler(ProductRepository productRepository, ImageService imageService)
            {
                _productRepository = productRepository;
                _imageService = imageService;
            }

            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                var product = await _productRepository.GetByIdAsync(request.Id);

                if (product == null)
                    throw new Exception("Product not found");

                _imageService.DeleteImage(product.ImagePath);


                await _productRepository.DeleteAsync(request.Id);
            }
        }
    }
}