using MediatR;
using Microsoft.AspNetCore.Mvc;
using api.Domain;
using api.Application.Products;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            return await _mediator.Send(new List.Query());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(string id)
        {
            var product = await _mediator.Send(new Details.Query { Id = id });
            
            if (product == null)
                return NotFound();
                
            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Create.Command command)
        {
            var product = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(string id, Update.Command command)
        {
            command.Id = id;
            
            try 
            {
                await _mediator.Send(command);
                return NoContent();
            }
            catch (Exception ex) when (ex.Message == "Product not found")
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            try 
            {
                await _mediator.Send(new Delete.Command { Id = id });
                return NoContent();
            }
            catch (Exception ex) when (ex.Message == "Product not found")
            {
                return NotFound();
            }
        }
    }
}