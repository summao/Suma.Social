using Microsoft.AspNetCore.Http;

namespace Suma.Social.Models.Persons
{
    public class UpdateProfileImageRequest 
    {
        public IFormFile ProfileImage { get; set; }
    }
}