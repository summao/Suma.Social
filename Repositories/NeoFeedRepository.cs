using System.Threading.Tasks;
using Neo4jClient;
using Suma.Social.Entities;

namespace Suma.Social.Repositories
{
    public class NeoFeedRepository
    {
        private readonly IGraphClient _graphClient;

        public NeoFeedRepository(IGraphClient graphClient)
        {
            _graphClient = graphClient;
        }

        public async Task<Feed> GetAsync()
        {
            var feeds = await _graphClient.Cypher.Match("(p:Person{Id:'1'})-[:Post]->(f:Feed)")
                .Return()
        }
    
    }
}