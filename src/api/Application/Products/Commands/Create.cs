using api.Domain;
using api.Persistence.Repositories;
using FluentValidation;
using MediatR;

namespace api.Application.Products.Commands
{
    public static class Create
    {
        public class Command : IRequest<Product>
        {
            public string Name { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public string Brand { get; set; } = string.Empty;
            public List<string> Categories { get; set; } = new();
            public IFormFile? Image { get; set; }

        }

        public class Handler : IRequestHandler<Command, Product>
        {
            private readonly ProductRepository _productRepository;
            private readonly IImageService _imageService;


            public Handler(ProductRepository productRepository, IImageService imageService)
            {
                _imageService = imageService;
                _productRepository = productRepository;
            }

            public async Task<Product> Handle(Command request, CancellationToken cancellationToken)
            {
                var product = Product.Create(
                    name: request.Name,
                    description: request.Description,
                    brand: request.Brand,
                    categories: request.Categories
                );

                if (request.Image != null)
                {
                    product.ImagePath = await _imageService.SaveImageAsync(request.Image);
                }

                await _productRepository.CreateAsync(product);
                return product;
            }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
                RuleFor(x => x.Description).MaximumLength(2000);
                RuleFor(x => x.Brand).MaximumLength(100);
                RuleFor(x => x.Categories).Must(x => x.Count <= 10)
                    .WithMessage("Cannot have more than 10 categories");
            }
        }
    }

}