namespace BackEndAje.Api.Application.Users.Commands.UploadUsers
{
    using System.Text.RegularExpressions;
    using BackEndAje.Api.Application.Interfaces;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;
    using OfficeOpenXml;

    public class UploadUsersHandler : IRequestHandler<UploadUsersCommand, UploadUsersResult>
    {
        private readonly ICediRepository _cediRepository;
        private readonly IZoneRepository _zoneRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHashingService _hashingService;

        public UploadUsersHandler(ICediRepository cediRepository, IZoneRepository zoneRepository, IRoleRepository roleRepository, IUserRepository userRepository, IHashingService hashingService)
        {
            this._cediRepository = cediRepository;
            this._zoneRepository = zoneRepository;
            this._roleRepository = roleRepository;
            this._userRepository = userRepository;
            this._hashingService = hashingService;
        }

        public async Task<UploadUsersResult> Handle(UploadUsersCommand request, CancellationToken cancellationToken)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var cedis = await this._cediRepository.GetAllCedis();
            var zones = await this._zoneRepository.GetAllZones();
            var roles = await this._roleRepository.GetAllRolesAsync();
            var positions = await this._roleRepository.GetAllPositionsAsync();

            var cediDict = cedis
                .GroupBy(c => c.CediName.ToUpper())
                .Select(g => g.First())
                .ToDictionary(c => c.CediName.ToUpper());

            var zoneDict = zones
                .GroupBy(z => z.ZoneCode)
                .Select(g => g.First())
                .ToDictionary(z => z.ZoneCode);

            var roleDict = roles
                .GroupBy(r => r.RoleName.ToUpper().Trim())
                .Select(g => g.First())
                .ToDictionary(r => r.RoleName.ToUpper().Trim());

            var positionDict = positions
                .GroupBy(r => r.PositionName.ToUpper().Trim())
                .Select(g => g.First())
                .ToDictionary(r => r.PositionName.ToUpper().Trim());

            using var memoryStream = new MemoryStream(request.FileBytes);
            using var package = new ExcelPackage(memoryStream);
            var worksheet = package.Workbook.Worksheets[0];

            var usersToAdd = new List<User>();
            var rolesToAssign = new List<(User User, int RoleId)>();
            var processedUsers = 0;
            var errors = new List<UploadError>();

            for (var row = 2; row <= worksheet.Dimension.End.Row; row++)
            {
                try
                {
                    var cediName = worksheet.Cells[row, 1].Text.ToUpper();
                    if (!cediDict.TryGetValue(cediName, out var cedi))
                    {
                        throw new KeyNotFoundException($"Tipo de documento '{cediName}' no encontrado en la fila {row}.");
                    }

                    var zoneCodeText = worksheet.Cells[row, 2].Text;
                    int? zoneCode = !string.IsNullOrWhiteSpace(zoneCodeText) && zoneCodeText != "-"
                        ? int.Parse(zoneCodeText)
                        : null;

                    Zone? zone = null;
                    if (zoneCode.HasValue)
                    {
                        if (!zoneDict.TryGetValue(zoneCode.Value, out zone))
                        {
                            throw new KeyNotFoundException($"Zona '{zoneCode}' no encontrada en la fila {row}.");
                        }
                    }

                    var zoneId = zone?.ZoneId;

                    var roleName = worksheet.Cells[row, 5].Text.ToUpper();
                    if (!roleDict.TryGetValue(roleName, out var role))
                    {
                        throw new KeyNotFoundException($"Rol '{roleName}' no encontrado en la fila {row}.");
                    }

                    var positionName = worksheet.Cells[row, 4].Text.ToUpper();
                    if (!positionDict.TryGetValue(positionName, out var position))
                    {
                        throw new KeyNotFoundException($"Posicion '{positionName}' no encontrado en la fila {row}.");
                    }

                    var routeText = worksheet.Cells[row, 3].Text;
                    int? route = !string.IsNullOrWhiteSpace(routeText) && routeText != "-"
                        ? int.Parse(routeText)
                        : null;

                    var documentNumber = Regex.Replace(worksheet.Cells[row, 9].Text, @"\D", "");
                    User existingUser = null;

                    if (route.HasValue)
                    {
                        existingUser = (await this._userRepository.GetUserByRouteAsync(route.Value))!;
                    }

                    existingUser ??= (await this._userRepository.GetUserByDocumentNumberAsync(documentNumber))!;

                    var user = existingUser ?? new User
                    {
                        IsActive = true,
                        CreatedAt = DateTime.Now,
                        CreatedBy = request.CreatedBy,
                    };

                    this.UpdateUserData(user, worksheet, row, cedi.CediId, position.PositionId, zoneId, route, request.UpdatedBy);
                    if (existingUser == null)
                    {
                        usersToAdd.Add(user);
                    }
                    else
                    {
                        user.UpdatedAt = DateTime.Now;
                        user.UpdatedBy = request.UpdatedBy;
                        await this._userRepository.UpdateUserAsync(user);
                    }

                    rolesToAssign.Add((user, role.RoleId));
                    processedUsers++;
                }
                catch (Exception ex)
                {
                    errors.Add(new UploadError
                    {
                        Row = row,
                        Message = ex.Message,
                    });
                }
            }

            if (usersToAdd.Any())
            {
                await this._userRepository.AddUsersAsync(usersToAdd);
            }

            var appUsersToAdd = new List<AppUser>();
            foreach (var (user, roleId) in rolesToAssign)
            {
                var routeOrEmail = !string.IsNullOrWhiteSpace(user.Route?.ToString())
                    ? user.Route.ToString() : !string.IsNullOrWhiteSpace(user.Email) ? user.Email : throw new InvalidOperationException($"No se proporcionó Route ni Email para el usuario con ID {user.UserId}.");

                var existingAppUser = await this._userRepository.GetAppUserByRouteOrEmailAsync(routeOrEmail!);
                if (existingAppUser == null)
                {
                    var appUser = new AppUser
                    {
                        UserId = user.UserId,
                        RouteOrEmail = routeOrEmail!,
                        PasswordHash = this._hashingService.HashPassword("1111"),
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        CreatedBy = request.CreatedBy,
                        UpdatedBy = request.UpdatedBy,
                    };
                    appUsersToAdd.Add(appUser);
                }

                var existingUserRole = await this._userRepository.GetUserRoleByUserIdAsync(user.UserId);
                if (existingUserRole == null)
                {
                    await this._userRepository.AddUserRoleAsync(user.UserId, roleId, request.CreatedBy, request.UpdatedBy);
                }
            }

            if (appUsersToAdd.Any())
            {
                await this._userRepository.AddAppUsersAsync(appUsersToAdd);
            }

            return new UploadUsersResult
            {
                Success = !errors.Any(),
                ProcessedClients = processedUsers,
                Errors = errors,
            };
        }

        private void UpdateUserData(User user, ExcelWorksheet worksheet, int row, int cediId, int positionId, int? zoneId, int? route, int updatedBy)
        {
            user.CediId = cediId;
            user.ZoneId = zoneId;
            user.Route = route;
            user.Names = worksheet.Cells[row, 8].Text;
            user.PositionId = positionId;
            user.PaternalSurName = worksheet.Cells[row, 6].Text;
            user.MaternalSurName = worksheet.Cells[row, 7].Text;
            user.Email = worksheet.Cells[row, 11].Text;
            user.Phone = worksheet.Cells[row, 10].Text.Replace(" ", "");
            user.DocumentNumber = Regex.Replace(worksheet.Cells[row, 9].Text, @"\D", "");
            user.IsActive = true;
            user.UpdatedAt = DateTime.Now;
            user.UpdatedBy = updatedBy;
        }
    }
}
