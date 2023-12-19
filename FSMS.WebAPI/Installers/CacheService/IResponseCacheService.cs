namespace FSMS.WebAPI.Installers.CacheService
{
    public interface IResponseCacheService
    {
        Task SetCacheResponseAsync(string cacheKey, object response, TimeSpan timeOut);
        Task<string> GetCachedResponseAsync(string cacheKey);

    }
}
