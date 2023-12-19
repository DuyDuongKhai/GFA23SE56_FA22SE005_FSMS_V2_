using FSMS.WebAPI.Configurations;
using FSMS.WebAPI.Installers.CacheService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using System.Text;

namespace FSMS.WebAPI.Attributes
{
    public class CacheAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLiveSeconds;

        public CacheAttribute(int timeToLiveSeconds = 1000)
        {
            _timeToLiveSeconds = timeToLiveSeconds;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            try
            {
                var cacheConfiguration = context.HttpContext.RequestServices.GetRequiredService<IOptions<RedisConfiguration>>().Value;
                if (cacheConfiguration.Enable)
                {
                    var cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();
                    var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);
                    var cacheResponse = await cacheService.GetCachedResponseAsync(cacheKey);

                    if (!string.IsNullOrEmpty(cacheResponse))
                    {
                        var contentResult = new ContentResult
                        {
                            Content = cacheResponse,
                            ContentType = "application/json",
                            StatusCode = 200
                        };
                        context.Result = contentResult;
                        return;
                    }
                }

                var executedContext = await next();

                if (executedContext.Result is OkObjectResult objectResult)
                {
                    var cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();
                    var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);
                    await cacheService.SetCacheResponseAsync(cacheKey, objectResult.Value, TimeSpan.FromSeconds(_timeToLiveSeconds));
                }
            }
            catch (Exception)
            {
                // Log or handle the exception as needed
            }
        }

        private static string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            var keyBuilder = new StringBuilder();
            keyBuilder.Append($"{Uri.EscapeDataString(request.Path)}");

            foreach (var (key, value) in request.Query.OrderBy(x => x.Key, StringComparer.OrdinalIgnoreCase))
            {
                keyBuilder.Append($"|{Uri.EscapeDataString(key)}-{Uri.EscapeDataString(value)}");
            }

            var cacheKey = keyBuilder.ToString();
            Console.WriteLine($"Generated cache key: {cacheKey}");
            return cacheKey;
        }

    }
}