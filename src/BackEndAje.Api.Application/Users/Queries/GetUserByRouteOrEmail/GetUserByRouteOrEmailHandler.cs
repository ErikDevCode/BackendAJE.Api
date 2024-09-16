namespace BackEndAje.Api.Application.Users.Queries.GetUserByRouteOrEmail
{
    using AutoMapper;
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
        private readonly IMapper _mapper;

        public GetUserByRouteOrEmailHandler(IUserRepository userRepository, IRoleRepository roleRepository, IPermissionRepository permissionRepository, IUserRoleRepository userRoleRepository, IRolePermissionRepository rolePermissionRepository, IRegionRepository regionRepository, ICediRepository cediRepository, IZoneRepository zoneRepository, IMapper mapper)
        {
            this._userRepository = userRepository;
            this._roleRepository = roleRepository;
            this._permissionRepository = permissionRepository;
            this._userRoleRepository = userRoleRepository;
            this._rolePermissionRepository = rolePermissionRepository;
            this._regionRepository = regionRepository;
            this._cediRepository = cediRepository;
            this._zoneRepository = zoneRepository;
            this._mapper = mapper;
        }

        public async Task<GetUserByRouteOrEmailResult> Handle(GetUserByRouteOrEmailQuery request, CancellationToken cancellationToken)
        {
            var user = await this.GetUserAsync(request.RouteOrEmail);

            var result = this._mapper.Map<GetUserByRouteOrEmailResult>(user);


            result.RegionName = (await this.GetRegionNameAsync(user.RegionId)) !;
            result.CediName = await this.GetCediNameAsync(user.CediId);
            result.ZoneCode = await this.GetZoneCodeAsync(user.ZoneId);
            result.Roles = await this.GetRolesWithPermissionsAsync(user.UserId);

            return result;
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

        private async Task<List<RoleResponse>> GetRolesWithPermissionsAsync(int userId)
        {
            var userRoles = await this._userRoleRepository.GetUserRolesAsync(userId);
            var roleIds = userRoles.Select(ur => ur.RoleId).ToList();

            var allRoles = await this._roleRepository.GetAllRolesAsync();
            var allRolePermissions = await this._rolePermissionRepository.GetAllRolePermissionsAsync();
            var allPermissions = await this._permissionRepository.GetAllPermissionsAsync();

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
                .ToList() !;
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

        private async Task<string?> GetRegionNameAsync(int? regionId)
        {
            if (regionId == null)
            {
                return null;
            }

            var regions = await this._regionRepository.GetAllPermissionsAsync();
            return regions.FirstOrDefault(x => x.RegionId == regionId)?.RegionName;
        }

        private async Task<string?> GetCediNameAsync(int? cediId)
        {
            if (cediId == null)
            {
                return null;
            }

            var cedi = await this._cediRepository.GetCediByCediIdAsync(cediId);
            return cedi?.CediName;
        }

        private async Task<int?> GetZoneCodeAsync(int? zoneId)
        {
            if (zoneId == null)
            {
                return null;
            }

            var zone = await this._zoneRepository.GetZoneByZoneIdAsync(zoneId);
            return zone?.ZoneCode;
        }
    }
}
