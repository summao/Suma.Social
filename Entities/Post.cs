using System;
using System.ComponentModel.DataAnnotations;

namespace Suma.Social.Entities
{
    public class Post : BaseEntity
    {
        public DateTimeOffset Created { get; set; }

        [RegularExpression(@"^(public|friends|onlyme)$")]
        public string PrivacyLevel { get; set; }
        public string Text { get; set; }
        public string ImageName { get; set; }
        public string ProfileImageName { get; set; }
        public int PosterId { get; set; }
    }
}