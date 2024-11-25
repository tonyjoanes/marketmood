using MediatR;
using api.Domain;
using api.Persistence.Repositories;

namespace api.Application.SourceReviews.Queries
{
    public class ListUnprocessed
    {
        public class Query : IRequest<List<SourceReview>> { }

        public class Handler : IRequestHandler<Query, List<SourceReview>>
        {
            private readonly ISourceReviewRepository _repository;
            private readonly ILogger<Handler> _logger;

            public Handler(ISourceReviewRepository repository, ILogger<Handler> logger)
            {
                _repository = repository;
                _logger = logger;
            }

            public async Task<List<SourceReview>> Handle(Query request, CancellationToken cancellationToken)
            {
                var statuses = new[] { ReviewStatus.New };
                var reviews = await _repository.GetReviewsByStatuses(statuses);
                return reviews.ToList();
            }
        }
    }

    public class Details
    {
        public class Query : IRequest<SourceReview>
        {
            public string Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, SourceReview>
        {
            private readonly ISourceReviewRepository _repository;

            public Handler(ISourceReviewRepository repository)
            {
                _repository = repository;
            }

            public async Task<SourceReview> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _repository.GetReviewById(request.Id);
            }
        }
    }
}
