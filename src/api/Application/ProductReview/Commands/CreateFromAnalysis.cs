using MediatR;
using api.Domain;
using api.Application.Reviews.DTOs;
using api.Persistence.Repositories;

namespace api.Application.ProductReviews.Commands
{
    public class CreateFromAnalysis
    {
        public class Command : IRequest<Domain.ProductReview>
        {
            public string SourceReviewId { get; set; }
            public ReviewAnalysisRequest Analysis { get; set; }
        }

        public class Handler : IRequestHandler<Command, Domain.ProductReview>
        {
            private readonly ISourceReviewRepository _sourceReviewRepository;
            private readonly IProductReviewRepository _productReviewRepository;
            private readonly IProductRepository _productRepository;
            private readonly ILogger<Handler> _logger;

            public Handler(
                ISourceReviewRepository sourceReviewRepository,
                IProductReviewRepository productReviewRepository,
                IProductRepository productRepository,
                ILogger<Handler> logger)
            {
                _sourceReviewRepository = sourceReviewRepository;
                _productReviewRepository = productReviewRepository;
                _productRepository = productRepository;
                _logger = logger;
            }

            public async Task<Domain.ProductReview> Handle(Command request, CancellationToken cancellationToken)
            {
                var sourceReview = await _sourceReviewRepository.GetReviewById(request.SourceReviewId);
                if (sourceReview == null)
                    throw new Exception($"Source review {request.SourceReviewId} not found");

                var themes = request.Analysis.Themes.Select(t =>
                    ThemeAnalysis.Create(
                        theme: t.Theme,
                        count: t.Count,
                        sentimentScore: t.SentimentScore
                    )).ToList();

                var productReview = Domain.ProductReview.CreateFromReview(
                    review: sourceReview,
                    sentimentScore: request.Analysis.SentimentScore,
                    keyPhrases: request.Analysis.KeyPhrases,
                    themes: themes
                );

                var savedReview = await _productReviewRepository.AddAsync(productReview);

                // Update source review status
                sourceReview.MarkAsProcessed();
                await _sourceReviewRepository.UpdateReview(sourceReview);

                // Update product analysis
                var product = await _productRepository.GetByIdAsync(sourceReview.ProductId);
                if (product != null)
                {
                    product.AddReview(savedReview);
                    await _productRepository.UpdateAsync(product.Id, product);
                }

                return savedReview;
            }
        }
    }
}
