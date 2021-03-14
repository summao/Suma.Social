using System;

namespace Suma.Social.Models.Comments
{
    public class CreateCommentRequest
    {
        public string ReplyToPostId { get; set; }
        public string Text { get; set; }
    }
}