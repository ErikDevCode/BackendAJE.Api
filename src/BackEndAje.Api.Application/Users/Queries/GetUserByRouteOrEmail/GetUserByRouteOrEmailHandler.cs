namespace BackEndAje.Api.Application.Users.Queries.GetUserByRouteOrEmail
{
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetUserByRouteIdOrEmailHandler : IRequestHandler<GetUserByRouteIdOrEmailQuery, GetUserByRouteIdOrEmailResult>
    {
        private readonly IUserRepository _userRepository;

        public GetUserByRouteIdOrEmailHandler(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }

        public async Task<GetUserByRouteIdOrEmailResult> Handle(GetUserByRouteIdOrEmailQuery request, CancellationToken cancellationToken)
        {
            var user = await this._userRepository.GetUserByEmailOrRouteAsync(request.RouteIdOrEmail);

            if (user == null)
            {
                throw new KeyNotFoundException($"User with email '{request.RouteIdOrEmail}' not found.");
            }

            var userName = $"{user.PaternalSurName} {user.MaternalSurName} {user.Names}";
            return new GetUserByRouteIdOrEmailResult(user.UserId, user.RegionId, user.CediId, user.ZoneId, user.Route, user.Code, user.PaternalSurName, user.MaternalSurName, user.Names, userName, user.Email!, user.Phone, user.IsActive, user.CreatedAt);
        }
    }
}
