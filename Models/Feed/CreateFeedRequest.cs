using System.ComponentModel.DataAnnotations;

namespace Suma.Social.Models.Feed
{
    public class CreateFeedRequest
    {
        public string Text { get; set; }

        [RegularExpression(@"^(public|friend|onlyme)$")]
        public string PrivacyLevel { get; set; }
    }
}