using api.Application.Products;
using api.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        public IMediator Mediator { get; set; }

        public ProductsController(IMediator mediator)
        {
            Mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            Console.WriteLine("GET /api/products");
            return await Mediator.Send(new List.Query());
        }
    }
}
