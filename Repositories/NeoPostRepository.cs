using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Neo4j.Driver;
using Suma.Social.Entities;

namespace Suma.Social.Repositories
{
    public interface INeoPostRepository
    {
        Task<IEnumerable<Post>> GetAsync(int postedUserId);
        Task<Post> InsertAsynce(Post post, int userId);
    }

    public class NeoPostRepository : INeoPostRepository
    {
        private readonly IDriver _driver;

        public NeoPostRepository(IDriver driver)
        {
            _driver = driver;
        }

        public async Task<IEnumerable<Post>> GetAsync(int postedUserId)
        {
            var session = _driver.AsyncSession();
            try
            {
                var cursor = await session.RunAsync(
                   @"MATCH (p:Person { id: $p.id })-[Post]->(f:Post) 
                    RETURN f.id as Id,
                        f.created as Created,
                        f.privacyLevel as PrivacyLevel,
                        f.text as Text,
                        f.imageName as ImageName,
                        p.profileImageName as ProfileImageName",
                   new
                   {
                       p = new Dictionary<string, object> {
                            {"id", postedUserId}
                       }
                   }
               );
                return await cursor.ToListAsync(r =>
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
            finally
            {
                await session.CloseAsync();
            }

        }

        public async Task<Post> InsertAsynce(Post post, int userId)
        {
            var session = _driver.AsyncSession();
            try
            {

                var parameters = post.AsDictionary();
                parameters.Add("userId", userId);

                var cursor = await session.RunAsync(
                    @"MATCH (p:Person{ id: $a.userId })
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
                        f.imageName as ImageName",
                    new { a = parameters }
                );

                return await cursor.SingleAsync(r =>
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
            finally
            {
                await session.CloseAsync();
            }
        }
    }
}