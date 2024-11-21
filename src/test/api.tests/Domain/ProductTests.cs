using Xunit;
using FluentAssertions;
using api.Domain;

namespace api.tests.Domain;

public class ProductTests
{
    [Fact]
    public void Create_WithValidInputs_ShouldCreateProduct()
    {
        // Arrange
        var brand = "Samsung";
        var model = "Galaxy S21";
        var type = "Smartphone";
        var sentiment = 0.85;
        var reviewCount = 100;
        var imageUrl = "https://example.com/image.jpg";

        // Act
        var product = Product.Create(brand, model, type, sentiment, reviewCount, imageUrl);

        // Assert
        product.Should().NotBeNull();
        product.Brand.Should().Be(brand);
        product.Model.Should().Be(model);
        product.Type.Should().Be(type);
        product.Sentiment.Should().Be(sentiment);
        product.ReviewCount.Should().Be(reviewCount);
        product.ImageUrl.Should().Be(imageUrl);
        product.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        product.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Theory]
    [InlineData("", "Model", "Invalid brand")]
    [InlineData("Brand", "", "Invalid model")]
    public void Create_WithInvalidInputs_ShouldThrowArgumentException(string brand, string model, string testCase)
    {
        // Act
        var action = () => Product.Create(brand, model, "type", 0.5, 10, "url");

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("*cannot be empty", because: testCase);
    }

    [Fact]
    public void Update_WithValidInputs_ShouldUpdateProduct()
    {
        // Arrange
        var product = Product.Create("Initial", "Initial", "type", 0.5, 10, "url");
        var newBrand = "Updated";
        var newModel = "UpdatedModel";
        var newType = "UpdatedType";
        var newSentiment = 0.9;
        var newReviewCount = 200;
        var newImageUrl = "new-url";

        // Act
        product.Update(newBrand, newModel, newType, newSentiment, newReviewCount, newImageUrl);

        // Assert
        product.Brand.Should().Be(newBrand);
        product.Model.Should().Be(newModel);
        product.Type.Should().Be(newType);
        product.Sentiment.Should().Be(newSentiment);
        product.ReviewCount.Should().Be(newReviewCount);
        product.ImageUrl.Should().Be(newImageUrl);
        product.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void AddUrl_ShouldAddUrlToList()
    {
        // Arrange
        var product = Product.Create("Brand", "Model", "type", 0.5, 10, "url");
        var domain = "amazon.com";
        var url = "https://amazon.com/product";

        // Act
        product.AddUrl(domain, url);

        // Assert
        product.Urls.Should().ContainSingle();
        product.Urls[0].Domain.Should().Be(domain);
        product.Urls[0].Url.Should().Be(url);
        product.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void AddReview_ShouldAddReviewToList()
    {
        // Arrange
        var product = Product.Create("Brand", "Model", "type", 0.5, 10, "url");
        var review = ProductReview.Create(
            "product-123",
            "review-456",
            5,
            "Great product",
            "This is amazing",
            "John Doe",
            DateTime.UtcNow,
            true,
            10
        );

        // Act
        product.AddReview(review);

        // Assert
        product.Reviews.Should().ContainSingle();
        var addedReview = product.Reviews[0];
        addedReview.Should().Be(review);
        addedReview.ProductId.Should().Be("product-123");
        addedReview.ReviewId.Should().Be("review-456");
        addedReview.Rating.Should().Be(5);
        addedReview.Title.Should().Be("Great product");
        addedReview.Content.Should().Be("This is amazing");
        addedReview.Author.Should().Be("John Doe");
        addedReview.VerifiedPurchase.Should().BeTrue();
        addedReview.HelpfulVotes.Should().Be(10);
        product.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void AddMultipleReviews_ShouldAddAllReviewsToList()
    {
        // Arrange
        var product = Product.Create("Brand", "Model", "type", 0.5, 10, "url");
        var review1 = ProductReview.Create("pid", "r1", 4, "Title 1", "Content 1", "Author 1", DateTime.UtcNow, true, 5);
        var review2 = ProductReview.Create("pid", "r2", 5, "Title 2", "Content 2", "Author 2", DateTime.UtcNow, false, 3);

        // Act
        product.AddReview(review1);
        product.AddReview(review2);

        // Assert
        product.Reviews.Should().HaveCount(2);
        product.Reviews.Should().Contain(review1);
        product.Reviews.Should().Contain(review2);
        product.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void UpdateImageUrl_ShouldUpdateImageUrlAndTimestamp()
    {
        // Arrange
        var product = Product.Create("Brand", "Model", "type", 0.5, 10, "url");
        var newImageUrl = "new-image-url";

        // Act
        product.UpdateImageUrl(newImageUrl);

        // Assert
        product.ImageUrl.Should().Be(newImageUrl);
        product.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void UpdateLastScrapedAt_ShouldUpdateScrapedTimestamp()
    {
        // Arrange
        var product = Product.Create("Brand", "Model", "type", 0.5, 10, "url");
        var scrapedAt = DateTime.UtcNow.AddHours(-1);

        // Act
        product.UpdateLastScrapedAt(scrapedAt);

        // Assert
        product.LastScrapedAt.Should().Be(scrapedAt);
        product.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }
}