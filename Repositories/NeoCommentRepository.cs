using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Neo4j.Driver;
using Suma.Social.Entities;
using Suma.Social.Models.Comments;

namespace Suma.Social.Repositories
{
    public interface INeoCommentRepository
    {
        Task<CreateCommentResponse> InsertAsync(Comment comment, int commentorId, string replyToPostId);
        Task<List<Comment>> GetManyAsync(string postId);
    }

    public class NeoCommentRepository : INeoCommentRepository
    {
        private readonly IDriver _driver;

        public NeoCommentRepository(IDriver driver)
        {
            _driver = driver;
        }

        public async Task<CreateCommentResponse> InsertAsync(Comment comment, int commentorId, string replyToPostId)
        {
            var session = _driver.AsyncSession();
            var parameters = comment.AsDictionary();
            parameters.Add("commentorId", commentorId);

            var query = @"MATCH (f:Post {id: $a.replyToPostId})
                MATCH (p:Person {id: $a.commentorId})
                CREATE (p)-[:Comment]->(c:Comment {
                    id: $a.id, 
                    text: $a.text,
                    created: datetime()
                })-[:Reply]->(f)
                RETURN c.id as CommentId,
                    c.text as Text,
                    c.created as Created,
                    f.id as ReplyToPostId,
                    p.id as CommentorId";

            parameters.Add("replyToPostId", replyToPostId);

            try
            {
                var cursor = await session.RunAsync(query, new { a = parameters });

                return await cursor.SingleAsync(r =>
                    new CreateCommentResponse
                    {
                        CommentId = r["CommentId"].As<string>(),
                        Text = r["Text"].As<string>(),
                        Created = r["Created"].As<DateTimeOffset>(),
                        ReplyToPostId = r["ReplyToPostId"].As<string>(),
                        CommentorId = r["CommentorId"].As<int>(),
                    }
                );
            }
            finally
            {
                await session.CloseAsync();
            }
        }

        public async Task<List<Comment>> GetManyAsync(string postId)
        {
            var session = _driver.AsyncSession();
            var parameters = new Dictionary<string, object> {
                {"postId", postId}
            };

            var query = @"MATCH (c:Comment)-[:Reply]->(f:Post {id: $a.postId})
                RETURN c.id as Id,
                    c.text as Text,
                    c.created as Created";

            try
            {
                var cursor = await session.RunAsync(query, new { a = parameters });

                return await cursor.ToListAsync(r =>
                    new Comment
                    {
                        Id = r["Id"].As<string>(),
                        Text = r["Text"].As<string>(),
                        Created = r["Created"].As<DateTimeOffset>(),
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