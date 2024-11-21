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
            public string Brand { get; set; } = string.Empty;
            public string Model { get; set; } = string.Empty;
            public string Type { get; set; } = string.Empty;
            public double Sentiment { get; set; } = 0;
            public int ReviewCount { get; set; } = 0;
            public string ImageUrl { get; set; } = "/images/products/placeholder.jpg";

        }

        public class Handler : IRequestHandler<Command, Product>
        {
            private readonly IProductRepository _productRepository;
            private readonly IImageService _imageService;


            public Handler(IProductRepository productRepository, IImageService imageService)
            {
                _imageService = imageService;
                _productRepository = productRepository;
            }

            public async Task<Product> Handle(Command request, CancellationToken cancellationToken)
            {
                var product = Product.Create(
                    brand: request.Brand,
                    model: request.Model,
                    type: request.Type,
                    sentiment: request.Sentiment,
                    reviewCount: request.ReviewCount,
                    imageUrl: request.ImageUrl
                );

                // if (request.Image != null)
                // {
                //     product.ImagePath = await _imageService.SaveImageAsync(request.Image);
                // }

                await _productRepository.CreateAsync(product);
                return product;
            }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Brand).NotEmpty().MaximumLength(200);
                RuleFor(x => x.Model).MaximumLength(2000);
                RuleFor(x => x.Type).MaximumLength(100);
                // RuleFor(x => x.Categories).Must(x => x.Count <= 10)
                //     .WithMessage("Cannot have more than 10 categories");
            }
        }
    }

}
