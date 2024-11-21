using MediatR;
using Microsoft.AspNetCore.Mvc;
using api.Domain;
using api.Application.ProductReview.Commands;
using api.Application.ProductReview.DTOs;
using api.Application.ProductReview.Queries;
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
        public async Task<ActionResult<List<ProductReview>>> GetReviewsByProduct(string productId)
        {
            return await _mediator.Send(new ListByProduct.Query { ProductId = productId });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductReview>> GetReview(string id)
        {
            var review = await _mediator.Send(new Details.Query { Id = id });

            if (review == null)
                return NotFound();

            return review;
        }

        [HttpPost("batch")]
        public async Task<ActionResult<BatchReviewResult>> CreateReviews(List<CreateReviewRequest> requests)
        {
            var command = new BatchCreate.Command { Reviews = requests };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<ProductReview>> CreateReview(CreateReviewRequest request)
        {
            var command = new Create.Command
            {
                ProductId = request.ProductId,
                ReviewId = request.ReviewId,
                Rating = request.Rating,
                Title = request.Title,
                Content = request.Content,
                Author = request.Author,
                Date = request.Date,
                VerifiedPurchase = request.VerifiedPurchase,
                HelpfulVotes = request.HelpfulVotes
            };

            var review = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetReview), new { id = review.Id }, review);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(string id)
        {
            try
            {
                await _mediator.Send(new Delete.Command { Id = id });
                return NoContent();
            }
            catch (Exception ex) when (ex.Message == "Review not found")
            {
                return NotFound();
            }
        }
    }
}
