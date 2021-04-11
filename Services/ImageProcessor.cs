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
        byte[] WriteTextOnImage();
    }

    public class ImageProcessor : IImageProcessor
    {
        public async Task<MemoryStream> Resize(MemoryStream memoryStream, int height)
        {
            var returnMemoryStream = new MemoryStream();
            using(Image image = await Image.LoadAsync(memoryStream))
            {
                if(image.Height > height)
                {
                    using(Image copy = image.Clone(a => a.Resize(0, height)))
                    {
                        await copy.SaveAsync(returnMemoryStream, new JpegEncoder());
                    }
                }
                else
                {
                    await image.SaveAsync(returnMemoryStream, new JpegEncoder());
                }
            }

            return returnMemoryStream;
        }

        public byte[] WriteTextOnImage()
        {
            var filePath = Path.Combine(Path.GetFullPath("Templates"), "sfinger.jpg");
            var bitmap = SKBitmap.Decode(filePath);

            using var canvas = new SKCanvas(bitmap);
            using var paint = new SKPaint();

            paint.Color = SKColors.Black;
            paint.TextSize = 25.0f;
            paint.IsAntialias = true;
            paint.Typeface = SKTypeface.FromFamilyName("Tahoma");

            canvas.DrawText("อีดอก", bitmap.Width / 2f, 110, paint);
            canvas.Flush();

            using var image = SKImage.FromBitmap(bitmap);
            using var data = image.Encode();

            return data.ToArray();    
        }
    }
}