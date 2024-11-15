using FluentAssertions;
using api.Domain;

namespace api.tests.Domain
{
    public class ProductTests
    {
        [Fact]
        public void Create_WithValidData_ShouldCreateProduct()
        {
            // Arrange
            var name = "Test Product";
            var description = "Test Description";
            var brand = "Test Brand";
            var categories = new List<string> { "Category1" };

            // Act
            var product = Product.Create(name, description, brand, categories);

            // Assert
            product.Should().NotBeNull();
            product.Name.Should().Be(name);
            product.Description.Should().Be(description);
            product.Brand.Should().Be(brand);
            product.Categories.Should().BeEquivalentTo(categories);
            product.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
            product.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
            product.Id.Should().BeNull();
            product.Urls.Should().BeEmpty();
            product.Reviews.Should().BeEmpty();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void Create_WithInvalidName_ShouldThrowArgumentException(string invalidName)
        {
            // Arrange
            var description = "Test Description";
            var brand = "Test Brand";
            var categories = new List<string> { "Category1" };

            // Act & Assert
            var act = () => Product.Create(invalidName, description, brand, categories);
            act.Should().Throw<ArgumentException>()
                .WithMessage("Name cannot be empty");
        }

        [Fact]
        public void Create_WithNullCategories_ShouldCreateEmptyCategoriesList()
        {
            // Arrange
            var name = "Test Product";
            var description = "Test Description";
            var brand = "Test Brand";

            // Act
            var product = Product.Create(name, description, brand, null);

            // Assert
            product.Categories.Should().NotBeNull();
            product.Categories.Should().BeEmpty();
        }

        [Fact]
        public void Update_WithValidData_ShouldUpdateProduct()
        {
            // Arrange
            var product = Product.Create("Initial Name", "Initial Description", "Initial Brand", new List<string> { "Initial Category" });
            var originalCreatedAt = product.CreatedAt;
            
            var newName = "Updated Name";
            var newDescription = "Updated Description";
            var newBrand = "Updated Brand";
            var newCategories = new List<string> { "New Category" };

            // Act
            product.Update(newName, newDescription, newBrand, newCategories);

            // Assert
            product.Name.Should().Be(newName);
            product.Description.Should().Be(newDescription);
            product.Brand.Should().Be(newBrand);
            product.Categories.Should().BeEquivalentTo(newCategories);
            product.CreatedAt.Should().Be(originalCreatedAt);
            product.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void Update_WithInvalidName_ShouldThrowArgumentException(string invalidName)
        {
            // Arrange
            var product = Product.Create("Initial Name", "Initial Description", "Initial Brand", new List<string> { "Initial Category" });

            // Act & Assert
            var act = () => product.Update(invalidName, "New Description", "New Brand", new List<string>());
            act.Should().Throw<ArgumentException>()
                .WithMessage("Name cannot be empty");
        }

        [Fact]
        public void AddUrl_ShouldAddNewUrl()
        {
            // Arrange
            var product = Product.Create("Test Product", "Description", "Brand", new List<string>());
            var domain = "example.com";
            var url = "https://example.com/product";

            // Act
            product.AddUrl(domain, url);

            // Assert
            product.Urls.Should().HaveCount(1);
            product.Urls[0].Domain.Should().Be(domain);
            product.Urls[0].Url.Should().Be(url);
            product.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [Fact]
        public void AddReview_ShouldAddNewReview()
        {
            // Arrange
            var product = Product.Create("Test Product", "Description", "Brand", new List<string>());
            var review = Review.Create("Test Reviewer", "Great product!", 5.0m);

            // Act
            product.AddReview(review);

            // Assert
            product.Reviews.Should().HaveCount(1);
            product.Reviews[0].Should().Be(review);
            product.Reviews[0].ReviewerName.Should().Be("Test Reviewer");
            product.Reviews[0].ReviewText.Should().Be("Great product!");
            product.Reviews[0].Rating.Should().Be(5.0m);
            product.Reviews[0].ReviewDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
            product.Reviews[0].Analysis.Should().BeNull();
            product.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [Fact]
        public void SetId_ShouldSetIdWhenNotAlreadySet()
        {
            // Arrange
            var product = Product.Create("Test Product", "Description", "Brand", new List<string>());
            var id = "test-id";

            // Act - Using reflection since SetId is internal
            typeof(Product).GetMethod("SetId", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.Invoke(product, new object[] { id });

            // Assert
            product.Id.Should().Be(id);
        }

        [Fact]
        public void SetId_WhenIdAlreadySet_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var product = Product.Create("Test Product", "Description", "Brand", new List<string>());
            var methodInfo = typeof(Product).GetMethod("SetId", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            methodInfo?.Invoke(product, new object[] { "initial-id" });

            // Act & Assert
            var act = () => methodInfo?.Invoke(product, new object[] { "new-id" });
            act.Should().Throw<System.Reflection.TargetInvocationException>()
                .WithInnerException<InvalidOperationException>()
                .WithMessage("Cannot change existing ID");
        }

        [Fact]
        public void UpdateAnalysis_ShouldUpdateAnalysisAndTimestamp()
        {
            // Arrange
            var product = Product.Create("Test Product", "Description", "Brand", new List<string>());
            var analysis = new ProductAnalysis();

            // Act
            product.UpdateAnalysis(analysis);

            // Assert
            product.Analysis.Should().Be(analysis);
            product.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [Fact]
        public void UpdateLastScrapedAt_ShouldUpdateScrapedAtAndTimestamp()
        {
            // Arrange
            var product = Product.Create("Test Product", "Description", "Brand", new List<string>());
            var scrapedAt = DateTime.UtcNow.AddHours(-1);

            // Act
            product.UpdateLastScrapedAt(scrapedAt);

            // Assert
            product.LastScrapedAt.Should().Be(scrapedAt);
            product.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [Fact]
        public void UpdateImageUrl_ShouldUpdateImagePathAndTimestamp()
        {
            // Arrange
            var product = Product.Create("Test Product", "Description", "Brand", new List<string>());
            var newImagePath = "/images/test.jpg";

            // Act
            product.UpdateImageUrl(newImagePath);

            // Assert
            product.ImagePath.Should().Be(newImagePath);
            product.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }
    }
}