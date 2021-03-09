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
    }
}