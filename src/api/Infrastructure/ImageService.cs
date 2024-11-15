namespace api.Infrastructure;

public class ImageService : IImageService
{
    private readonly string _imageDirectory;
    private readonly IWebHostEnvironment _environment;
    private readonly string _baseUrl;

    public ImageService(
        IWebHostEnvironment environment,
        IConfiguration configuration)
    {
        _environment = environment;
        _imageDirectory = Path.Combine(_environment.WebRootPath, "images");
        _baseUrl = configuration["BaseUrl"] ?? "http://localhost:5000";

        if (!Directory.Exists(_imageDirectory))
        {
            Directory.CreateDirectory(_imageDirectory);
        }
    }

    public async Task<string> SaveImageAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("Invalid file");

        var allowedTypes = new[] { "image/jpeg", "image/png", "image/gif" };
        if (!allowedTypes.Contains(file.ContentType.ToLower()))
            throw new ArgumentException("Invalid file type");

        if (file.Length > 5 * 1024 * 1024)
            throw new ArgumentException("File size exceeds limit");

        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var filePath = Path.Combine(_imageDirectory, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return $"/images/{fileName}";
    }

    public void DeleteImage(string? imagePath)
    {
        if (string.IsNullOrEmpty(imagePath))
            return;

        var fullPath = Path.Combine(_environment.WebRootPath, imagePath.TrimStart('/'));
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }

    public string? GetImageUrl(string? imagePath)
    {
        if (string.IsNullOrEmpty(imagePath))
            return null;

        return $"{_baseUrl}{imagePath}";
    }
}