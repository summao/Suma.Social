using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Neo4j.Driver;
using Suma.Social.Entities;

namespace Suma.Social.Repositories
{
    public interface INeoPostRepository
    {
        Task<Post> GetOneAsyncByPostId(string postId);
        Task<IEnumerable<Post>> GetManyAsyncByPosterId(int posterId);
        Task<Post> InsertAsynce(Post post, int userId);
    }

    public class NeoPostRepository : BaseNeoRepository, INeoPostRepository
    {
        public NeoPostRepository(IDriver driver) : base(driver)
        {

        }

        public async Task<Post> GetOneAsyncByPostId(string postId)
        {
            var query = @"MATCH (p:Person)-[Post]->(f:Post {id: $a.postId})
                RETURN f.id as Id,
                    f.privacyLevel as PrivacyLevel,
                    f.text as Text,
                    f.created as Created,
                    p.id as PosterId";

            var parameters = new Dictionary<string, object> {
                {"postId", postId}
            };

            return await ExecuteSingleAsync(query, parameters, r =>
                new Post
                {
                    Id = r["Id"].As<string>(),
                    PrivacyLevel = r["PrivacyLevel"].As<string>(),
                    Text = r["Text"].As<string>(),
                    Created = r["Created"].As<DateTimeOffset>(),
                    PosterId = r["PosterId"].As<int>(),
                }
            );
        }

        public async Task<IEnumerable<Post>> GetManyAsyncByPosterId(int posterId)
        {
            var query = @"MATCH (p:Person { id: $p.id })-[Post]->(f:Post) 
                    RETURN f.id as Id,
                        f.created as Created,
                        f.privacyLevel as PrivacyLevel,
                        f.text as Text,
                        f.imageName as ImageName,
                        p.profileImageName as ProfileImageName";

            var parameters = new Dictionary<string, object> {
                 {"id", posterId}
            };

            return await ExecuteListAsync(query, parameters, r =>
                new Post
                {
                    Id = r["Id"].As<string>(),
                    Created = r["Created"].As<DateTimeOffset>(),
                    PrivacyLevel = r["PrivacyLevel"].As<string>(),
                    Text = r["Text"].As<string>(),
                    ImageName = r["ImageName"].As<string>(),
                    ProfileImageName = r["ProfileImageName"].As<string>(),
                }
            );

        }

        public async Task<Post> InsertAsynce(Post post, int userId)
        {
            var query = @"MATCH (p:Person{ id: $a.userId })
                    CREATE (p)-[:Post]->(f:Post {
                        id: $a.id , 
                        created: datetime(),
                        privacyLevel: $a.privacyLevel,
                        text: $a.text,
                        imageName: $a.imageName
                    })
                    RETURN f.id as Id,
                        f.created as Created,
                        f.privacyLevel as PrivacyLevel,
                        f.text as Text,
                        f.imageName as ImageName";

            var parameters = post.AsDictionary();
            parameters.Add("userId", userId);

            return await ExecuteSingleAsync(query, parameters, r =>
                new Post
                {
                    Id = r["Id"].As<string>(),
                    Created = r["Created"].As<DateTimeOffset>(),
                    PrivacyLevel = r["PrivacyLevel"].As<string>(),
                    Text = r["Text"].As<string>(),
                    ImageName = r["ImageName"].As<string>(),
                }
            );
        }
    }
}