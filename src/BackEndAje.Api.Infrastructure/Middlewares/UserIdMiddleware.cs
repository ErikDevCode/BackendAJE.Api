namespace BackEndAje.Api.Infrastructure.Middlewares
{
    using System.Security.Claims;
    using Microsoft.AspNetCore.Http;

    public class UserIdMiddleware
    {
        private readonly RequestDelegate _next;

        public UserIdMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLower();

            // Excluir rutas públicas
            if (path != null && (path.Contains("/auth/login") || path.Contains("/public")))
            {
                await this._next(context);
                return;
            }

            try
            {
                var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier) ?? context.User.FindFirst("sub");

                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId))
                {
                    context.Items["UserId"] = userId;
                }
                else
                {
                    // Opcional: puedes configurar esto en null si no se encuentra el claim
                    context.Items["UserId"] = null;
                }
            }
            catch
            {
                context.Items["UserId"] = null;
            }

            await this._next(context);
        }
    }
}