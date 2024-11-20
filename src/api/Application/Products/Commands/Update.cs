using api.Application.Common.Exceptions;
using api.Persistence.Repositories;
using FluentValidation;
using MediatR;

namespace api.Application.Products.Commands
{
    public class Update
    {
        public class Command : IRequest
        {
            public string Id { get; set; } = string.Empty;
            public string Model { get; set; } = string.Empty;
            public string Type { get; set; } = string.Empty;
            public string Brand { get; set; } = string.Empty;
            public double Sentiment { get; set; } = 0;
            public string ImageUrl { get; set; } = string.Empty;
            public int ReviewCount { get; set; } = 0;

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
                    throw new ProductNotFoundException(request.Id);

                product.Update(
                    brand: request.Brand,
                    model: request.Model,
                    type: request.Type,
                    reviewCount: request.ReviewCount,
                    sentiment: request.Sentiment,
                    imageUrl: request.ImageUrl
                );

                // if (request.Image != null)
                // {
                //     var newImagePath = await _imageService.SaveImageAsync(request.Image);
                //     _imageService.DeleteImage(product.ImagePath);
                //     product.UpdateImageUrl(newImagePath);
                // }

                await _productRepository.UpdateAsync(request.Id, product);
            }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Id).NotEmpty();
                RuleFor(x => x.Brand)
                    .NotEmpty()
                    .MaximumLength(200);
                RuleFor(x => x.Model)
                    .MaximumLength(2000);
                RuleFor(x => x.Type)
                    .MaximumLength(100);
                // RuleFor(x => x.Categories)
                //     .Must(x => x.Count <= 10)
                //     .WithMessage("Cannot have more than 10 categories");
            }
        }
    }
}
