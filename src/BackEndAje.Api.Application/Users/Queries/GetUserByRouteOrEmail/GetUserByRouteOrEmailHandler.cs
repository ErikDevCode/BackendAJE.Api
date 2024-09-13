namespace BackEndAje.Api.Application.Users.Queries.GetUserByRouteOrEmail
{
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetUserByRouteOrEmailHandler : IRequestHandler<GetUserByRouteOrEmailQuery, GetUserByRouteOrEmailResult>
    {
        private readonly IUserRepository _userRepository;

        public GetUserByRouteOrEmailHandler(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }

        public async Task<GetUserByRouteOrEmailResult> Handle(GetUserByRouteOrEmailQuery request, CancellationToken cancellationToken)
        {
            var user = await this._userRepository.GetUserByEmailOrRouteAsync(request.RouteOrEmail);

            if (user == null)
            {
                throw new KeyNotFoundException($"User with email '{request.RouteOrEmail}' not found.");
            }

            var userName = $"{user.PaternalSurName} {user.MaternalSurName} {user.Names}";
            return new GetUserByRouteOrEmailResult(user.UserId, user.RegionId, user.CediId, user.ZoneId, user.Route, user.Code, user.PaternalSurName, user.MaternalSurName, user.Names, userName, user.Email!, user.Phone, user.IsActive, user.CreatedAt);
        }
    }
}
