using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Suma.Social.Repositories;

namespace Suma.Social.Services
{
    public interface IImageService
    {
        /// <returns>A string of file name</returns>
        Task<string> AddOneAsync(IFormFile file);
        Task<byte[]> GetOneAsync(string imageName);
    }

    public class ImageService : IImageService
    {
        private readonly IImageRepository _imageRepository;
        private readonly IImageProcessor _imageProcessor;

        public ImageService(
            IImageRepository imageRepository,
            IImageProcessor imageProcessor
        )
        {
            _imageRepository = imageRepository;
            _imageProcessor = imageProcessor;
        }

        public async Task<string> AddOneAsync(IFormFile file)
        {
            var mStream = new MemoryStream();
            file.CopyTo(mStream);
            mStream.Position = 0;
            var result = await _imageProcessor.Resize(mStream, 540);
            return await _imageRepository.InsertOneAsync(result);
        }

        public async Task<byte[]> GetOneAsync(string imageName)
        {
            return await _imageRepository.GetOneAsync(imageName);
        }
    }
}