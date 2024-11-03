namespace api.Models
{
    public class ProductReviewDatabaseSettings : IProductReviewDatabaseSettings
    {
        public string ProductsCollectionName { get; set; }
        public string ReviewsCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IProductReviewDatabaseSettings
    {
        string ProductsCollectionName { get; set; }
        string ReviewsCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}