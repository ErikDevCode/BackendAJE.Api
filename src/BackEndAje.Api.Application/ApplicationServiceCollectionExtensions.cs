namespace BackEndAje.Api.Application
{
    using BackEndAje.Api.Application.Abstractions.Users;
    using BackEndAje.Api.Application.Services;
    using Microsoft.Extensions.DependencyInjection;

    public static class ApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUserLoginService, UserLoginService>();
            services.AddTransient<NotificationNewService>();
            services.AddTransient<RelocationService>();
            services.AddTransient<OrderService>();
            return services;
        }
    }
}
