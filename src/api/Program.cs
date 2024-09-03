using api.Models;
using api.Services;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<ProductReviewDatabaseSettings>(
    builder.Configuration.GetSection(nameof(ProductReviewDatabaseSettings)));

builder.Services.AddSingleton<IProductReviewDatabaseSettings>(sp =>
    sp.GetRequiredService<IOptions<ProductReviewDatabaseSettings>>().Value);

builder.Services.AddSingleton<ProductService>();
builder.Services.AddSingleton<ReviewService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
