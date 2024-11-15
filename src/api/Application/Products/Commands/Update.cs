using api.Persistence;
using api.Persistence.Repositories;
using FluentValidation;
using MediatR;

namespace api.Application.Products
{
    public class Update
    {
        public class Command : IRequest
        {
            public string Id { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public string Brand { get; set; } = string.Empty;
            public List<string> Categories { get; set; } = new();
            public IFormFile? Image { get; set; }

        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly ProductRepository _productRepository;
            private readonly IImageService _imageService;


            public Handler(ProductRepository productRepository, IImageService imageService)
            {
                _productRepository = productRepository;
                _imageService = imageService;

            }

            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                var product = await _productRepository.GetByIdAsync(request.Id);

                if (product == null)
                    throw new ProductNotFoundException(request.Id);

                product.Update(
                    name: request.Name,
                    description: request.Description,
                    brand: request.Brand,
                    categories: request.Categories
                );

                if (request.Image != null)
                {
                    var newImagePath = await _imageService.SaveImageAsync(request.Image);
                    _imageService.DeleteImage(product.ImagePath);
                    product.UpdateImageUrl(newImagePath);
                }

                await _productRepository.UpdateAsync(request.Id, product);
            }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Id).NotEmpty();
                RuleFor(x => x.Name)
                    .NotEmpty()
                    .MaximumLength(200);
                RuleFor(x => x.Description)
                    .MaximumLength(2000);
                RuleFor(x => x.Brand)
                    .MaximumLength(100);
                RuleFor(x => x.Categories)
                    .Must(x => x.Count <= 10)
                    .WithMessage("Cannot have more than 10 categories");
            }
        }
    }
}