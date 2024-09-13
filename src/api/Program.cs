using api.Application.Products;
using api.Models;
using api.Persistence;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<ProductReviewDatabaseSettings>(
    builder.Configuration.GetSection(nameof(ProductReviewDatabaseSettings)));

builder.Services.AddSingleton<IProductReviewDatabaseSettings>(sp =>
    sp.GetRequiredService<IOptions<ProductReviewDatabaseSettings>>().Value);

builder.Services.AddScoped<ProductRepository>();

// Register MongoDB client and database as services
builder.Services.AddSingleton<IMongoClient, MongoClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<ProductReviewDatabaseSettings>>().Value;
    return new MongoClient(settings.ConnectionString);
});

builder.Services.AddScoped(sp =>
{
    var settings = sp.GetRequiredService<IOptions<ProductReviewDatabaseSettings>>().Value;
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase(settings.DatabaseName);
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(List.Query).Assembly));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
