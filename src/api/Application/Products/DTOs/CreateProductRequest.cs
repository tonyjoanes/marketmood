namespace api.Application.Products.DTOs
{
    public class CreateProductRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public List<string> Categories { get; set; } = new();
        public IFormFile? Image { get; set; }
    }
}