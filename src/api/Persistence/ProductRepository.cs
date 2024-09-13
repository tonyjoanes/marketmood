using api.Domain;
using MongoDB.Driver;

namespace api.Persistence
{
    public class ProductRepository
    {
        private readonly IMongoCollection<Product> _productsCollection;

        public ProductRepository(IMongoDatabase database)
        {
            // Access the "Products" collection
            _productsCollection = database.GetCollection<Product>("Products");
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _productsCollection.Find(_ => true).ToListAsync();
        }

        public async Task<Product> GetByIdAsync(string id)
        {
            return await _productsCollection.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Product product)
        {
            await _productsCollection.InsertOneAsync(product);
        }

        public async Task UpdateAsync(string id, Product updatedProduct)
        {
            await _productsCollection.ReplaceOneAsync(p => p.Id == id, updatedProduct);
        }

        public async Task DeleteAsync(string id)
        {
            await _productsCollection.DeleteOneAsync(p => p.Id == id);
        }
    }
}