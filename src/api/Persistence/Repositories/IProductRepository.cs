namespace api.Persistence
{
    using api.Domain;

    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(string id);
        Task CreateAsync(Product product);
        Task UpdateAsync(string id, Product updatedProduct);
        Task DeleteAsync(string id);
    }
}