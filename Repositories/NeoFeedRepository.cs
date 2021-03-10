using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Neo4j.Driver;
using Suma.Social.Entities;

namespace Suma.Social.Repositories
{
    public interface INeoFeedRepository
    {
        Task<IEnumerable<Feed>> GetAsync(int postedUserId);
        Task<Feed> InsertAsynce(Feed feed, int userId);
    }

    public class NeoFeedRepository : INeoFeedRepository
    {
        private readonly IDriver _driver;

        public NeoFeedRepository(IDriver driver)
        {
            _driver = driver;
        }

        public async Task<IEnumerable<Feed>> GetAsync(int postedUserId)
        {
            var session = _driver.AsyncSession();
            IResultCursor cursor;
            var feeds = new List<Feed>();
            try
            {
                cursor = await session.RunAsync(
                    @"MATCH (p:Person { id: $p.id })-[Post]->(f:Feed) 
                    RETURN f.id as id,
                        f.text as text,
                        f.created as created",
                    new
                    {
                        p = new Dictionary<string, object> {
                            {"id", postedUserId}
                        }
                    }
                );
                feeds = await cursor.ToListAsync(r =>
                {
                    return new Feed
                    {
                        Id = r["id"].As<string>(),
                        Text = r["text"].As<string>(),
                        Created = r["created"].As<DateTimeOffset>(),
                    };
                });
            }
            finally
            {
                await session.CloseAsync();
            }

            return feeds;
        }

        public async Task<Feed> InsertAsynce(Feed feed, int userId)
        {
            var session = _driver.AsyncSession();
            IResultCursor cursor;
            try
            {

                var parameters = feed.AsDictionary();
                parameters.Add("userId", userId);

                cursor = await session.RunAsync(
                    @"MATCH (p:Person{ id: $a.userId })
                    CREATE (p)-[:Post]->(f:Feed {
                        id: $a.id , 
                        text: $a.text, 
                        created: datetime(),
                        privacyLevel: $a.privacyLevel})
                    RETURN f.id as id,
                        f.text as text,
                        f.created as created,
                        f.privacyLevel as privacyLevel",
                    new { a = parameters }
                );

                return await cursor.SingleAsync(r =>
                    new Feed
                    {
                        Id = r["id"].As<string>(),
                        Text = r["text"].As<string>(),
                        Created = r["created"].As<DateTimeOffset>(),
                        PrivacyLevel = r["privacyLevel"].As<string>(),
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