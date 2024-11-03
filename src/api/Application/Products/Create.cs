using api.Domain;
using api.Persistence;
using FluentValidation;
using MediatR;
using MongoDB.Driver;

namespace api.Application.Products
{
    public static class Create
    {
        public class Command : IRequest<Product>
        {
            public string Name { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public string Brand { get; set; } = string.Empty;
            public List<string> Categories { get; set; } = new();
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
                var product = Product.Create(
                    name: request.Name,
                    description: request.Description,
                    brand: request.Brand,
                    categories: request.Categories
                );

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