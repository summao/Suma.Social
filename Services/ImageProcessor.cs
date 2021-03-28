using System.IO;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using SkiaSharp;

namespace Suma.Social.Services
{
    public interface IImageProcessor
    {
        Task<MemoryStream> Resize(MemoryStream memoryStream, int height);
    }

    public class ImageProcessor : IImageProcessor
    {
        public async Task<MemoryStream> Resize(MemoryStream memoryStream, int height)
        {
            var returnMemoryStream = new MemoryStream();
            using (Image image = await Image.LoadAsync(memoryStream))
            {
                if (image.Height > height)
                {
                    using (Image copy = image.Clone(a => a.Resize(0, height)))
                    {
                        await copy.SaveAsync(returnMemoryStream, new JpegEncoder());
                    }
                }
            }

            return returnMemoryStream;
        }

        public async Task WriteTextOnImage()
        {
            var filePath = Path.Combine(Path.GetFullPath("Templates"), "sfinger.jpg");
            var bitmap = SKBitmap.Decode(filePath);
        }
    }
}