using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using StackExchange.Redis;

namespace FSMS.WebAPI.Installers.CacheService
{
    public class ResponseCacheService : IResponseCacheService
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IConnectionMultiplexer _connectionMultiplexer;


        public ResponseCacheService(IDistributedCache distributedCache, IConnectionMultiplexer connectionMultiplexer)
        {
            _distributedCache = distributedCache;
            _connectionMultiplexer = connectionMultiplexer;
        }

        public async Task<string> GetCachedResponseAsync(string cacheKey)
        {
            try
            {
                var cachedResponse = await _distributedCache.GetStringAsync(cacheKey);

                if (!string.IsNullOrWhiteSpace(cachedResponse))
                {
                    return cachedResponse;
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving cached response for key {cacheKey}: {ex.Message}");
                return null;
            }
        }

        public async Task SetCacheResponseAsync(string cacheKey, object response, TimeSpan timeOut)
        {
            if (response == null)
                return;

            try
            {
                var serialierResponse = JsonConvert.SerializeObject(response, new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

                var database = _connectionMultiplexer.GetDatabase();

                await database.StringSetAsync(cacheKey, serialierResponse, timeOut);
            }
            catch (TimeoutException tex)
            {
                Console.WriteLine($"Timeout connecting to Redis: {tex.Message}");
                // Handle the timeout exception as needed (e.g., log, retry, etc.)
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error converting object to JSON and caching with key {cacheKey}: {ex.Message}");
            }
            finally
            {
                // Do not dispose the connectionMultiplexer here, let the DI container manage its lifecycle
            }
        }







    }
}
