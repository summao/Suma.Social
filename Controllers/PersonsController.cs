using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Suma.Social.Models.Persons;
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
        public async Task<IActionResult> UpdateProfileImage([FromForm] UpdateProfileImageRequest dto)
        {
            var profileImageName = await _personService.UpdateProfileImageAsync(dto.ProfileImage, userId);
            var resDto = new UpdateProfileImageResponse 
            {
                ProfileImageName = profileImageName
            };
            
            return Ok(resDto);
        }
    }
}
