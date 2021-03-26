using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Suma.Social.Models.Posts
{
    public class CreatePostRequest
    {
        [StringLength(10000)]
        public string Text { get; set; }

        [RegularExpression(Suma.Social.AppConstants.RegularExpressions.PRIVACY_LEVEL)]
        public string PrivacyLevel { get; set; }

        public IFormFile Image { get; set; }
    }
}