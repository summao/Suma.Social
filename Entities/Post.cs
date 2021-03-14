using System;
using System.ComponentModel.DataAnnotations;

namespace Suma.Social.Entities
{
    public class Post : BaseEntity
    {
        public string Text { get; set; }
        public DateTimeOffset Created { get; set; }

        [RegularExpression(@"^(public|friend|onlyme)$")]
        public string PrivacyLevel { get; set; }

    }
}