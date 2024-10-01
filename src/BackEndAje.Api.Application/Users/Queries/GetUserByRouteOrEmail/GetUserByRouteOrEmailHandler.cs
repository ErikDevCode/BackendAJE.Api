namespace BackEndAje.Api.Application.Users.Queries.GetUserByRouteOrEmail
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetUserByRouteOrEmailHandler : IRequestHandler<GetUserByRouteOrEmailQuery, GetUserByRouteOrEmailResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICediRepository _cediRepository;
        private readonly IZoneRepository _zoneRepository;
        private readonly IMapper _mapper;

        public GetUserByRouteOrEmailHandler(IUserRepository userRepository, ICediRepository cediRepository, IZoneRepository zoneRepository, IMapper mapper)
        {
            this._userRepository = userRepository;
            this._cediRepository = cediRepository;
            this._zoneRepository = zoneRepository;
            this._mapper = mapper;
        }

        public async Task<GetUserByRouteOrEmailResult> Handle(GetUserByRouteOrEmailQuery request, CancellationToken cancellationToken)
        {
            var user = await this.GetUserAsync(request.RouteOrEmail);

            var result = this._mapper.Map<GetUserByRouteOrEmailResult>(user);

            result.CediName = await this.GetCediNameAsync(user.CediId);
            result.ZoneCode = await this.GetZoneCodeAsync(user.ZoneId);

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
