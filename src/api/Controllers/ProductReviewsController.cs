using MediatR;
using Microsoft.AspNetCore.Mvc;
using api.Domain;
using api.Application.ProductReviews.Queries;
using api.Application.ProductReviews.Commands;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductReviewsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductReviewsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("product/{productId}")]
        public async Task<ActionResult<List<ProductReview>>> GetProductReviews(string productId)
        {
            return await _mediator.Send(new Application.ProductReview.Queries.ListByProduct.Query { ProductId = productId });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductReview>> GetReview(string id)
        {
            var review = await _mediator.Send(new GetProductReview.Query { Id = id });
            if (review == null) return NotFound();
            return review;
        }

        [HttpPost]
        public async Task<ActionResult<ProductReview>> CreateOrUpdate(CreateOrUpdateProductReview.Command command)
        {
            return await _mediator.Send(command);
        }
    }
}
