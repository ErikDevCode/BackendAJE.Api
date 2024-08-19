namespace BackEndAje.Api.Infrastructure.Middlewares
{
    using System.Net;
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json;

    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await this._next(httpContext);
            }
            catch (Exception ex)
            {
                await this.HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode statusCode;
            string message;

            switch (exception)
            {
                case ArgumentException _:
                    statusCode = HttpStatusCode.BadRequest;
                    message = exception.Message;
                    break;
                case KeyNotFoundException _:
                    statusCode = HttpStatusCode.NotFound;
                    message = exception.Message;
                    break;
                case UnauthorizedAccessException _:
                    statusCode = HttpStatusCode.Unauthorized;
                    message = "Invalid credentials.";
                    break;
                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    message = "An unexpected error occurred.";
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var result = JsonConvert.SerializeObject(new { Message = message });
            return context.Response.WriteAsync(result);
        }
    }
}
