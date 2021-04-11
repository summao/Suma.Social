using System.Collections.Generic;
using System.Threading.Tasks;
using Neo4j.Driver;

namespace Suma.Social.Repositories
{
    public interface IPersonNeoRepository
    {
        /// <returns>A string of profile image name</return>
        Task UpdateProfileImageNameAsync(string profileImageName, int personId);
    }

    public class PersonNeoRepository : BaseNeoRepository, IPersonNeoRepository
    {
        public PersonNeoRepository(IDriver driver) : base(driver)
        {

        }

        public async Task UpdateProfileImageNameAsync(string profileImageName, int personId)
        {
            var query = @"MATCH (p:Person{id: $a.personId }) 
                SET p.profileImageName = $a.profileImageName";

            var parameters = new Dictionary<string, object>{
                {"personId", personId},
                {"profileImageName", profileImageName},
            };

            await ExecuteNoneQueryAsync(query, parameters);
        }
    }
}