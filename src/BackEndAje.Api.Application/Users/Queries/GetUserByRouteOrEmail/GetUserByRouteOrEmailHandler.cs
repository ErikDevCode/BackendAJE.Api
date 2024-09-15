namespace BackEndAje.Api.Application.Users.Queries.GetUserByRouteOrEmail
{
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetUserByRouteOrEmailHandler : IRequestHandler<GetUserByRouteOrEmailQuery, GetUserByRouteOrEmailResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly IRegionRepository _regionRepository;
        private readonly ICediRepository _cediRepository;
        private readonly IZoneRepository _zoneRepository;

        public GetUserByRouteOrEmailHandler(IUserRepository userRepository, IRoleRepository roleRepository, IPermissionRepository permissionRepository, IUserRoleRepository userRoleRepository, IRolePermissionRepository rolePermissionRepository, IRegionRepository regionRepository, ICediRepository cediRepository, IZoneRepository zoneRepository)
        {
            this._userRepository = userRepository;
            this._roleRepository = roleRepository;
            this._permissionRepository = permissionRepository;
            this._userRoleRepository = userRoleRepository;
            this._rolePermissionRepository = rolePermissionRepository;
            this._regionRepository = regionRepository;
            this._cediRepository = cediRepository;
            this._zoneRepository = zoneRepository;
        }

        public async Task<GetUserByRouteOrEmailResult> Handle(GetUserByRouteOrEmailQuery request, CancellationToken cancellationToken)
        {
            var user = await this.GetUserAsync(request.RouteOrEmail);
            var userName = $"{user.PaternalSurName} {user.MaternalSurName} {user.Names}";

            var userRoles = await this.GetUserRolesAsync(user.UserId);
            var roleIds = userRoles.Select(ur => ur.RoleId).ToList();

            var allRoles = await this._roleRepository.GetAllRolesAsync();
            var allRolePermissions = await this._rolePermissionRepository.GetAllRolePermissionsAsync();
            var allPermissions = await this._permissionRepository.GetAllPermissionsAsync();

            var roles = this.GetRolesWithPermissions(roleIds, allRoles, allRolePermissions, allPermissions);

            var regions = await this._regionRepository.GetAllPermissionsAsync();
            var regionName = regions.FirstOrDefault(x => x.RegionId == user.RegionId)?.RegionName;

            string? cediName = null;
            if (user.CediId != null)
            {
                var cedi = await this._cediRepository.GetCediByCediIdAsync(user.CediId);
                cediName = cedi.CediName;
            }

            int? zoneCode = null;
            if (user.ZoneId != null)
            {
                var zone = await this._zoneRepository.GetZoneByZoneIdAsync(user.ZoneId);
                zoneCode = zone.ZoneCode;
            }

            return new GetUserByRouteOrEmailResult(
                user.UserId,
                user.RegionId,
                regionName!,
                user.CediId,
                cediName,
                user.ZoneId,
                zoneCode,
                user.Route,
                user.Code,
                user.PaternalSurName,
                user.MaternalSurName,
                user.Names,
                userName,
                user.Email!,
                user.Phone,
                user.IsActive,
                user.CreatedAt,
                roles);
        }

        private async Task<User> GetUserAsync(string routeOrEmail)
        {
            var user = await this._userRepository.GetUserByEmailOrRouteAsync(routeOrEmail);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with email or route '{routeOrEmail}' not found.");
            }

            return user;
        }

        private async Task<List<UserRole>> GetUserRolesAsync(int userId)
        {
            return await this._userRoleRepository.GetUserRolesAsync(userId);
        }

        private List<RoleResponse> GetRolesWithPermissions(
            List<int> roleIds,
            List<Role> allRoles,
            List<RolePermission> allRolePermissions,
            List<Permission> allPermissions)
        {
            return roleIds
                .Select(roleId =>
                {
                    var role = allRoles.FirstOrDefault(r => r.RoleId == roleId);

                    var rolePermissions = allRolePermissions
                        .Where(rp => rp.RoleId == roleId)
                        .Select(rp => rp.PermissionId)
                        .ToList();

                    var permissions = this.GetPermissionsForRole(rolePermissions, allPermissions);

                    return new RoleResponse
                    {
                        RoleName = role!.RoleName,
                        Permissions = permissions,
                    };
                })
                .Where(r => r != null)
                .ToList()!;
        }

        private List<PermissionResponse> GetPermissionsForRole(List<int> rolePermissions, List<Permission> allPermissions)
        {
            return rolePermissions
                .Select(permissionId =>
                {
                    var permission = allPermissions.FirstOrDefault(p => p.PermissionId == permissionId);
                    return permission;
                })
                .Where(p => p != null)
                .GroupBy(p => p.PermissionName)
                .Select(group =>
                {
                    var actions = new ActionPermissions
                    {
                        Read = group.Any(p => p.Action == "Read"),
                        Write = group.Any(p => p.Action == "Write"),
                        Update = group.Any(p => p.Action == "Update"),
                        Delete = group.Any(p => p.Action == "Delete"),
                    };

                    return new PermissionResponse
                    {
                        Module = group.Key,
                        Actions = actions,
                    };
                })
                .ToList();
        }
    }
}
