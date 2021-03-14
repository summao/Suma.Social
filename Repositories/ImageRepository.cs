using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Suma.Social.Repositories
{
    public interface IImageRepository
    {
        Task<string> InsertOneAsync(IFormFile file);
        Task<byte[]> GetOneAsync(string fileName);
    }

    public class ImageRepository : IImageRepository
    {
        private const string FILE_PATH = "UploadImages";

        public async Task<string> InsertOneAsync(IFormFile file)
        {
            var fileName = Path.GetRandomFileName();
            var filePath = Path.Combine(Path.GetFullPath(FILE_PATH), fileName);

            using (var stream = File.Create(filePath))
            {
                await file.CopyToAsync(stream);
            }

            return fileName;
        }

        public async Task<byte[]> GetOneAsync(string fileName)
        {
            var filePath = Path.Combine(Path.GetFullPath(FILE_PATH), fileName);
            return await File.ReadAllBytesAsync(filePath);
        }
    }
}