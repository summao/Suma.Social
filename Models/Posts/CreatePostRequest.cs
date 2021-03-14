using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Suma.Social.Models.Posts
{
    public class CreatePostRequest
    {
        public string Text { get; set; }

        [RegularExpression(@"^(public|friend|onlyme)$")]
        public string PrivacyLevel { get; set; }

        public IFormFile Image { get; set; }
    }
}