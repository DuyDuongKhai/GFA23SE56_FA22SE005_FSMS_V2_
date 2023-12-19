using FSMS.WebAPI.Middlewares;

public static class TimeZoneCorrectionMiddlewareExtensions
{
    public static IApplicationBuilder UseTimeZoneCorrection(this IApplicationBuilder builder)
    {
        // Set the offset for Vietnam time zone (GMT+7)
        int clientOffsetHours = 7;
        return builder.UseMiddleware<TimeZoneCorrectionMiddleware>(clientOffsetHours);
    }
}