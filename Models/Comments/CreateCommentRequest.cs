using System;

namespace Suma.Social.Models.Comments
{
    public class CreateCommentRequest
    {
        public string replyToFeedId { get; set; }
        public string Text { get; set; }
    }
}