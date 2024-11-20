namespace api.Application.Products.DTOs
{
    public class CreateProductRequest
    {
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public double Sentiment { get; set; } = 0;
        public int ReviewCount { get; set; } = 0;
        public string ImageUrl { get; set; } = string.Empty;
    }
}
