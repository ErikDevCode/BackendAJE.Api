namespace BackEndAje.Api.Infrastructure.Middlewares
{
    using System.Net;
    using FluentValidation;
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
            object response;

            switch (exception)
            {
                case ValidationException validationException:
                    statusCode = HttpStatusCode.BadRequest;
                    response = new { Errors = validationException.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }) };
                    break;
                case ArgumentException _:
                    statusCode = HttpStatusCode.BadRequest;
                    response = new { Message = exception.Message };
                    break;
                case KeyNotFoundException _:
                    statusCode = HttpStatusCode.NotFound;
                    response = new { Message = exception.Message };
                    break;
                case UnauthorizedAccessException _:
                    statusCode = HttpStatusCode.Unauthorized;
                    response = new { Message = "Invalid credentials." };
                    break;
                case InvalidOperationException _:
                    statusCode = HttpStatusCode.BadRequest;
                    response = new { Message = exception.Message };
                    break;
                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    response = new { Message = "An unexpected error occurred." };
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var result = JsonConvert.SerializeObject(response);
            return context.Response.WriteAsync(result);
        }
    }
}
