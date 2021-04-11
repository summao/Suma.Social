using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Suma.Social.Services;

namespace Suma.Social.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonsController : ControllerBase
    {
        private readonly IPersonService _personService;
        public static int userId = 1;

        public PersonsController(IPersonService personService)
        {
            _personService = personService;
        }

        [HttpPut("profile-image")]
        public async Task<IActionResult> UploadProfileImage(IFormFile profileImage)
        {
            var profileImageName = await _personService.UpdateProfileImageAsync(profileImage, userId);
            return Ok(profileImageName);
        }
    }
}
