using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Suma.Social.Services;

namespace Suma.Social.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImagesController : ControllerBase
    {
        private readonly IImageService _imageService;

        public ImagesController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            var imageName = await _imageService.AddOneAsync(file);

            return Created(imageName, null);
        }

        [HttpGet("{imageName}")]
        public async Task<IActionResult> Get(string imageName)
        {
            try
            {
                var image = await _imageService.GetOneAsync(imageName);
                return File(image, "image/jpeg");
            }
            catch (FileNotFoundException)
            {
                return BadRequest($"file not found :{imageName}");
            }
        }
    }
}