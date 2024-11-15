using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using FluentAssertions;
using api.Controllers;
using api.Domain;
using api.Application.Products;
using api.Application.Products.Commands;
using api.Application.Products.DTOs;
using api.Application.Products.Queries;

namespace api.tests.Controllers;

public class ProductsControllerTests
{
    private readonly Mock<IMediator> _mediator;
    private readonly ProductsController _controller;

    public ProductsControllerTests()
    {
        _mediator = new Mock<IMediator>();
        _controller = new ProductsController(_mediator.Object);
    }

    [Fact]
    public async Task GetProducts_ReturnsListOfProducts()
    {
        // Arrange
        var product = Product.Create(
            name: "Test Product",
            description: "Test Description",
            brand: "Test Brand",
            categories: new List<string> { "Category1" }
        );
        // Use reflection to set the Id since it's internal
        typeof(Product).GetMethod("SetId", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.Invoke(product, new object[] { "1" });

        var expectedProducts = new List<Product> { product };
        
        _mediator.Setup(m => m.Send(It.IsAny<List.Query>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedProducts);

        // Act
        var result = await _controller.GetProducts();

        // Assert
        result.Value.Should().BeEquivalentTo(expectedProducts);
    }

    [Fact]
    public async Task GetProduct_ReturnsProduct_WhenProductExists()
    {
        // Arrange
        var productId = "test-id";
        var product = Product.Create(
            name: "Test Product",
            description: "Test Description",
            brand: "Test Brand",
            categories: new List<string> { "Category1" }
        );
        // Use reflection to set the Id
        typeof(Product).GetMethod("SetId", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.Invoke(product, new object[] { productId });

        _mediator.Setup(m => m.Send(It.Is<Details.Query>(q => q.Id == productId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        // Act
        var result = await _controller.GetProduct(productId);

        // Assert
        result.Value.Should().BeEquivalentTo(product);
    }

    [Fact]
    public async Task GetProduct_ReturnsNotFound_WhenProductDoesNotExist()
    {
        // Arrange
        var productId = "nonexistent-id";
        _mediator.Setup(m => m.Send(It.Is<Details.Query>(q => q.Id == productId), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product)null);

        // Act
        var result = await _controller.GetProduct(productId);

        // Assert
        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task CreateProduct_ReturnsCreatedAtAction_WhenProductIsValid()
    {
        // Arrange
        var request = new CreateProductRequest
        {
            Name = "Test Product",
            Description = "Test Description",
            Brand = "Test Brand",
            Categories = new List<string> { "Category1" }
        };
        
        var createdProduct = Product.Create(
            name: request.Name,
            description: request.Description,
            brand: request.Brand,
            categories: request.Categories
        );
        // Use reflection to set the Id
        typeof(Product).GetMethod("SetId", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.Invoke(createdProduct, new object[] { "new-id" });

        _mediator.Setup(m => m.Send(
            It.Is<Create.Command>(c => 
                c.Name == request.Name && 
                c.Description == request.Description &&
                c.Brand == request.Brand &&
                c.Categories == request.Categories),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdProduct);

        // Act
        var result = await _controller.CreateProduct(request);

        // Assert
        var createdAtActionResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
        createdAtActionResult.ActionName.Should().Be(nameof(ProductsController.GetProduct));
        createdAtActionResult.RouteValues["id"].Should().Be("new-id");
        createdAtActionResult.Value.Should().BeEquivalentTo(createdProduct);
    }

    [Fact]
    public async Task UpdateProduct_ReturnsNoContent_WhenUpdateSucceeds()
    {
        // Arrange
        var productId = "test-id";
        var request = new UpdateProductRequest
        {
            Name = "Updated Product",
            Description = "Updated Description",
            Brand = "Updated Brand",
            Categories = new List<string> { "UpdatedCategory" }
        };

        _mediator.Setup(m => m.Send(
            It.Is<Update.Command>(c =>
                c.Id == productId &&
                c.Name == request.Name &&
                c.Description == request.Description &&
                c.Brand == request.Brand &&
                c.Categories == request.Categories),
            It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UpdateProduct(productId, request);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task DeleteProduct_ReturnsNoContent_WhenDeleteSucceeds()
    {
        // Arrange
        var productId = "test-id";
        _mediator.Setup(m => m.Send(
            It.Is<Delete.Command>(c => c.Id == productId),
            It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteProduct(productId);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task DeleteProduct_ReturnsNotFound_WhenProductDoesNotExist()
    {
        // Arrange
        var productId = "nonexistent-id";
        _mediator.Setup(m => m.Send(
            It.IsAny<Delete.Command>(),
            It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Product not found"));

        // Act
        var result = await _controller.DeleteProduct(productId);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }
}