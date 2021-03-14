using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Suma.Social.Repositories;

namespace Suma.Social.Services
{
    public interface IImageService
    {
        Task<string> AddOneAsync(IFormFile file);
        Task<byte[]> GetOneAsync(string imageName);
    }

    public class ImageService : IImageService
    {
        private readonly IImageRepository _imageRepository;

        public ImageService(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        public async Task<string> AddOneAsync(IFormFile file)
        {
            return await _imageRepository.InsertOneAsync(file);
        }

        public async Task<byte[]> GetOneAsync(string imageName)
        {
            return await _imageRepository.GetOneAsync(imageName);
        }
    }
}