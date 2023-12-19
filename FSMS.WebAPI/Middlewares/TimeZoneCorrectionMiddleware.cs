namespace FSMS.WebAPI.Middlewares
{
    public class TimeZoneCorrectionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly int clientOffsetHours; // Time zone offset in hours

        public TimeZoneCorrectionMiddleware(RequestDelegate next, int clientOffsetHours)
        {
            _next = next;
            this.clientOffsetHours = clientOffsetHours;
        }

        public async Task Invoke(HttpContext context)
        {
            // Get the current time of the server in UTC
            DateTime serverTime = DateTime.UtcNow;

            // Calculate and add the time zone offset for Vietnam (GMT+7)
            DateTime clientTime = serverTime.AddHours(clientOffsetHours);

            // Set the new value for the "date" header with the Vietnam time zone
            context.Response.Headers["date"] = clientTime.ToString("r");

            // Pass control to the next middleware in the pipeline
            await _next(context);
        }
    }
}
