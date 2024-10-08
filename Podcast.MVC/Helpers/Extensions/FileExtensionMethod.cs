using Microsoft.AspNetCore.Hosting;

namespace Podcast.MVC.Helpers.Extensions
{
    public static class FileExtensionMethod
    {
        public static bool IsImage(this IFormFile file)
        {
            return file.ContentType.Contains("image");
        }

        public static bool CheckSize(this IFormFile file, int mb)
        {
            return file.Length < mb * 10124 * 1024;
        }

        public static async Task<string> GenerateFileAsync(this IFormFile file, string path)
        {
            var imageName = $"{Guid.NewGuid()}-{file.FileName}";

            path = Path.Combine(path, imageName);

            var fs = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(fs);
            fs.Close();

            return imageName;
        }
    }
}
