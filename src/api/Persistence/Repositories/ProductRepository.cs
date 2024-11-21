using api.Domain;
using api.Persistence.Entities;
using MongoDB.Driver;

namespace api.Persistence.Repositories
{
    public class ProductRepository : IProductRepository
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
            var product = Product.Create(
                brand: entity.Brand,
                model: entity.Model,
                type: entity.Type,
                reviewCount: entity.ReviewCount,
                sentiment: entity.Sentiment,
                imageUrl: entity.ImageUrl ?? "/images/products/placeholder.jpg"
            );

            product.SetId(entity.Id);
            return product;
        }

        private static ProductEntity MapToEntity(Product domain)
        {
            if (domain == null)
                throw new ArgumentNullException(nameof(domain));

            return new ProductEntity
            {
                Id = domain.Id,  // Can be null for new products
                Brand = domain.Brand ?? throw new ArgumentException("Name cannot be null"),
                Model = domain.Model ?? string.Empty,
                Type = domain.Type,
                ReviewCount = domain.ReviewCount,
                Sentiment = domain.Sentiment,
                ImageUrl = domain.ImageUrl,
                CreatedAt = domain.CreatedAt,
                UpdatedAt = domain.UpdatedAt,
                LastScrapedAt = domain.LastScrapedAt  // DateTime? can be null
            };
        }
    }
}
