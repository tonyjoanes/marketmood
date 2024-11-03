using api.Domain;
using api.Persistence.Entities;
using MongoDB.Driver;

namespace api.Persistence
{
    public class ProductRepository
    {
        private readonly IMongoCollection<ProductEntity> _productsCollection;

        public ProductRepository(IMongoDatabase database)
        {
            _productsCollection = database.GetCollection<ProductEntity>("Products");
        }

        public async Task<List<Domain.Product>> GetAllAsync()
        {
            var entities = await _productsCollection.Find(_ => true).ToListAsync();
            return entities.Select(MapToDomain).ToList();
        }

        public async Task<Domain.Product> GetByIdAsync(string id)
        {
            var entity = await _productsCollection.Find(p => p.Id == id).FirstOrDefaultAsync();
            return entity != null ? MapToDomain(entity) : null;
        }

        public async Task CreateAsync(Domain.Product product)
        {
            var entity = MapToEntity(product);
            await _productsCollection.InsertOneAsync(entity);
            // Update the ID in the domain object after creation
            product.SetId(entity.Id);
        }

        public async Task UpdateAsync(string id, Domain.Product product)
        {
            var entity = MapToEntity(product);
            entity.Id = id;
            await _productsCollection.ReplaceOneAsync(p => p.Id == id, entity);
        }

        public async Task DeleteAsync(string id)
        {
            await _productsCollection.DeleteOneAsync(p => p.Id == id);
        }

        private static Product MapToDomain(ProductEntity entity)
        {
            var product = Product.Create(entity.Name);
            product.SetId(entity.Id);
            return product;
        }

        private static ProductEntity MapToEntity(Product product)
        {
            return new ProductEntity
            {
                Id = product.Id, // Will be null for new products
                Name = product.Name,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt
            };
        }
    }
}