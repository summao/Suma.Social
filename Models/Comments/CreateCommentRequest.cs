using System;

using System.ComponentModel.DataAnnotations;
namespace Suma.Social.Models.Comments
{
    public class CreateCommentRequest
    {
        [StringLength(36)]
        public string ReplyToPostId { get; set; }

        [StringLength(1000)]
        public string Text { get; set; }
    }
}