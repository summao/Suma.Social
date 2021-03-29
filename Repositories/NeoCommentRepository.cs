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

    public class NeoCommentRepository : BaseNeoRepository, INeoCommentRepository
    {

        public NeoCommentRepository(IDriver driver) : base(driver)
        {

        }

        public async Task<CreateCommentResponse> InsertAsync(Comment comment, int commentorId, string replyToPostId)
        {

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

            var parameters = comment.AsDictionary();
            parameters.Add("commentorId", commentorId);
            parameters.Add("replyToPostId", replyToPostId);

            return await ExecuteSingleAsync(query, parameters, r =>
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

        public async Task<List<Comment>> GetManyAsync(string postId)
        {
            var parameters = new Dictionary<string, object> {
                {"postId", postId}
            };

            var query = @"MATCH (c:Comment)-[:Reply]->(f:Post {id: $a.postId})
                RETURN c.id as Id,
                    c.text as Text,
                    c.created as Created";

            return await ExecuteListAsync(query, parameters,r =>
                new Comment
                {
                    Id = r["Id"].As<string>(),
                    Text = r["Text"].As<string>(),
                    Created = r["Created"].As<DateTimeOffset>(),
                }
            );
        }
    }
}