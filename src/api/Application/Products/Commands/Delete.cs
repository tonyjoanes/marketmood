using api.Infrastructure;
using api.Persistence.Repositories;
using MediatR;

namespace api.Application.Products.Commands
{
    public static class Delete
    {
        public class Command : IRequest
        {
            public string Id { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly IProductRepository _productRepository;
            private readonly IImageService _imageService;

            public Handler(IProductRepository productRepository, IImageService imageService)
            {
                _productRepository = productRepository;
                _imageService = imageService;
            }

            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                var product = await _productRepository.GetByIdAsync(request.Id);

                if (product == null)
                    throw new Exception("Product not found");

                // _imageService.DeleteImage(product.ImagePath);

                await _productRepository.DeleteAsync(request.Id);
            }
        }
    }
}
