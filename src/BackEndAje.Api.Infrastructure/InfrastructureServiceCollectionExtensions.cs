namespace BackEndAje.Api.Infrastructure
{
    using BackEndAje.Api.Application.Interfaces;
    using BackEndAje.Api.Domain.Repositories;
    using BackEndAje.Api.Infrastructure.Data;
    using BackEndAje.Api.Infrastructure.Repositories;
    using BackEndAje.Api.Infrastructure.Services;
    using BackEndAje.Api.Infrastructure.Services.Security;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class InfrastructureServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(
                    configuration.GetConnectionString("DefaultConnection"),
                    new MySqlServerVersion(new Version(8, 0, 21)),
                    options => options.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped<IS3Service, S3Service>();
            services.AddScoped<IHashingService, HashingService>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            services.AddScoped<IRolePermissionRepository, RolePermissionRepository>();
            services.AddScoped<IRegionRepository, RegionRepository>();
            services.AddScoped<ICediRepository, CediRepository>();
            services.AddScoped<IZoneRepository, ZoneRepository>();
            services.AddScoped<IClaimsTransformation, CustomClaimsTransformation>();
            services.AddScoped<IActionRepository, ActionRepository>();
            services.AddScoped<IMenuRepository, MenuRepository>();
            services.AddScoped<IMastersRepository, MastersRepository>();
            services.AddScoped<IUbigeoRepository, UbigeoRepository>();
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IOrderRequestRepository, OrderRequestRepository>();
            services.AddScoped<IPositionRepository, PositionRepository>();
            services.AddScoped<IAssetRepository, AssetRepository>();
            services.AddScoped<IClientAssetRepository, ClientAssetRepository>();
            services.AddScoped<ICensusRepository, CensusRepository>();

            return services;
        }
    }
}
