using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Neo4j.Driver;

namespace Suma.Social.Repositories
{
    public class BaseNeoRepository
    {
        private readonly IDriver _driver;

        public BaseNeoRepository(IDriver driver)
        {
            _driver = driver;
        }

        public async Task ExecuteNoneQueryAsync(string query, Dictionary<string, object> parameters)
        {
            var session = _driver.AsyncSession();
            try
            {
                await session.RunAsync(query, new { a = parameters });
            }
            finally
            {
                await session.CloseAsync();
            }
        }

        public async Task<T> ExecuteSingleAsync<T>(string query, Dictionary<string, object> parameters, Func<IRecord, T> func)
        {
            var session = _driver.AsyncSession();
            try
            {
                var cursor = await session.RunAsync(query, new { a = parameters });

                return await cursor.SingleAsync<T>(func);
            }
            finally
            {
                await session.CloseAsync();
            }
        }

        public async Task<List<T>> ExecuteListAsync<T>(string query, Dictionary<string, object> parameters, Func<IRecord, T> func)
        {
            var session = _driver.AsyncSession();
            try
            {
                var cursor = await session.RunAsync(query, new { a = parameters });

                return await cursor.ToListAsync<T>(func);
            }
            finally
            {
                await session.CloseAsync();
            }
        }

    }
}

