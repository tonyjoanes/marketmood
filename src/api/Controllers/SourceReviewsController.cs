using MediatR;
using Microsoft.AspNetCore.Mvc;
using api.Domain;
using api.Application.SourceReviews.Queries;
using api.Application.SourceReviews.Commands;
using api.Application.Reviews.DTOs;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SourceReviewsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SourceReviewsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("unprocessed")]
        public async Task<ActionResult<List<SourceReview>>> GetUnprocessedReviews()
        {
            return await _mediator.Send(new ListUnprocessed.Query());
        }

        [HttpPost("batch")]
        public async Task<ActionResult<BatchReviewResult>> CreateReviews(List<CreateSourceReviewRequest> requests)
        {
            var command = new BatchCreate.Command { Reviews = requests };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SourceReview>> GetReview(string id)
        {
            var review = await _mediator.Send(new Details.Query { Id = id });
            if (review == null) return NotFound();
            return review;
        }
    }
}
