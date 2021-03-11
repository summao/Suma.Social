using System;
using System.Threading.Tasks;
using Neo4j.Driver;
using Suma.Social.Entities;
using Suma.Social.Models.Comments;

namespace Suma.Social.Repositories
{
    public interface INeoCommentRepository
    {
        Task<CreateCommentResponse> InsertAsync(Comment comment, int commentorId, string replyToFeedId);
    }

    public class NeoCommentRepository : INeoCommentRepository
    {
        private readonly IDriver _driver;

        public NeoCommentRepository(IDriver driver)
        {
            _driver = driver;
        }

        public async Task<CreateCommentResponse> InsertAsync(Comment comment, int commentorId, string replyToFeedId)
        {
            var session = _driver.AsyncSession();
            var parameters = comment.AsDictionary();
            parameters.Add("commentorId", commentorId);

            var query = @"MATCH (f:Feed {id: $a.replyToFeedId})
                MATCH (p:Person {id: $a.commentorId})
                CREATE (p)-[:Comment]->(c:Comment {
                    id: $a.id, 
                    text: $a.text,
                    created: datetime()
                })-[:Reply]->(f)
                RETURN c.id as CommentId,
                    c.text as Text,
                    c.created as Created,
                    f.id as ReplyToFeedId,
                    p.id as CommentorId";

            parameters.Add("replyToFeedId", replyToFeedId);

            try
            {
                var cursor = await session.RunAsync(query, new { a = parameters });

                return await cursor.SingleAsync(r =>
                    new CreateCommentResponse
                    {
                        CommentId = r["CommentId"].As<string>(),
                        Text = r["Text"].As<string>(),
                        Created = r["Created"].As<DateTimeOffset>(),
                        ReplyToFeedId = r["ReplyToFeedId"].As<string>(),
                        CommentorId = r["CommentorId"].As<int>(),
                    }
                );
            }
            finally
            {
                await session.CloseAsync();
            }
        }
    }
}