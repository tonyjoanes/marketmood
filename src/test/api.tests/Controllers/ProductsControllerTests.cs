using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using FluentAssertions;
using api.Controllers;
using api.Domain;
using api.Application.Products.Commands;
using api.Application.Products.DTOs;
using api.Application.Products.Queries;
using api.Application.Common.Exceptions;

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
            brand: "Test Brand",
            model: "Test Model",
            type: "Test Type",
            sentiment: 0.85,
            reviewCount: 100,
            imageUrl: "test.jpg"
        );

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
            brand: "Test Brand",
            model: "Test Model",
            type: "Test Type",
            sentiment: 0.85,
            reviewCount: 100,
            imageUrl: "test.jpg"
        );

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
            Brand = "Test Brand",
            Model = "Test Model",
            Type = "Test Type",
            Sentiment = 0.85,
            ReviewCount = 100,
            ImageUrl = "test.jpg"
        };

        var createdProduct = Product.Create(
            brand: request.Brand,
            model: request.Model,
            type: request.Type,
            sentiment: request.Sentiment,
            reviewCount: request.ReviewCount,
            imageUrl: request.ImageUrl
        );

        _mediator.Setup(m => m.Send(
            It.Is<Create.Command>(c =>
                c.Brand == request.Brand &&
                c.Model == request.Model &&
                c.Type == request.Type &&
                c.Sentiment == request.Sentiment &&
                c.ReviewCount == request.ReviewCount &&
                c.ImageUrl == request.ImageUrl),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdProduct);

        // Act
        var result = await _controller.CreateProduct(request);

        // Assert
        var createdAtActionResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
        createdAtActionResult.ActionName.Should().Be(nameof(ProductsController.GetProduct));
        createdAtActionResult.Value.Should().BeEquivalentTo(createdProduct);
    }

    [Fact]
    public async Task UpdateProduct_ReturnsNoContent_WhenUpdateSucceeds()
    {
        // Arrange
        var productId = "test-id";
        var request = new UpdateProductRequest
        {
            Brand = "Updated Brand",
            Model = "Updated Model",
            Type = "Updated Type",
            Sentiment = 0.95,
            ReviewCount = 200,
            ImageUrl = "updated.jpg"
        };

        _mediator.Setup(m => m.Send(
            It.Is<Update.Command>(c =>
                c.Id == productId &&
                c.Brand == request.Brand &&
                c.Model == request.Model &&
                c.Type == request.Type &&
                c.Sentiment == request.Sentiment &&
                c.ReviewCount == request.ReviewCount &&
                c.ImageUrl == request.ImageUrl),
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
            It.Is<Delete.Command>(c => c.Id == productId),
            It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ProductNotFoundException(productId));

        // Act
        var result = await _controller.DeleteProduct(productId);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }
}