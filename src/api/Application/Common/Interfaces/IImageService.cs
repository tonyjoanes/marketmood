public interface IImageService
{
    Task<string> SaveImageAsync(IFormFile file);
    void DeleteImage(string? imagePath);
    string? GetImageUrl(string? imagePath);
}