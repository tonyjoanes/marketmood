using MongoDB.Driver;
using api.Models;

namespace api.Services
{
    public class ReviewService
    {
        private readonly IMongoCollection<Review> _reviews;

        public ReviewService(IProductReviewDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _reviews = database.GetCollection<Review>(settings.ReviewsCollectionName);
        }

        public List<Review> Get() => _reviews.Find(review => true).ToList();

        public Review Get(string id) => _reviews.Find<Review>(review => review.Id == id).FirstOrDefault();

        public Review Create(Review review)
        {
            _reviews.InsertOne(review);
            return review;
        }

        public void Update(string id, Review reviewIn) => _reviews.ReplaceOne(review => review.Id == id, reviewIn);

        public void Remove(Review reviewIn) => _reviews.DeleteOne(review => review.Id == reviewIn.Id);

        public void Remove(string id) => _reviews.DeleteOne(review => review.Id == id);
    }
}
