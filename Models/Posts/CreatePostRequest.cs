using System.ComponentModel.DataAnnotations;

namespace Suma.Social.Models.Posts
{
    public class CreatePostRequest
    {
        public string Text { get; set; }

        [RegularExpression(@"^(public|friend|onlyme)$")]
        public string PrivacyLevel { get; set; }
    }
}