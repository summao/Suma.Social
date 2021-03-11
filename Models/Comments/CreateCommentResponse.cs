using System;

namespace Suma.Social.Models.Comments
{
    public class CreateCommentResponse
    {
        public string CommentId { get; set; }
        public string Text { get; set; }
        public DateTimeOffset Created { get; set; }
        public string ReplyToFeedId { get; set; }
        public int CommentorId { get; set; }
    }
}