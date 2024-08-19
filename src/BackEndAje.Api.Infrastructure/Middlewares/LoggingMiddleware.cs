namespace BackEndAje.Api.Infrastructure.Middlewares
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;

    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            this._next = next;
            this._logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                this._logger.LogInformation("Handling request: {Method} {Path}", context.Request.Method, context.Request.Path);
                await this._next(context);
                this._logger.LogInformation("Finished handling request.");
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "An error occurred while processing the request.");
                throw;
            }
        }
    }
}
