namespace BackEndAje.Api.Application.Users.Queries.GetUsersByParam
{
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetUsersByParamHandler : IRequestHandler<GetUsersByParamQuery, List<GetUsersByParamResult>>
    {
        private readonly IUserRepository _userRepository;

        public GetUsersByParamHandler(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }

        public async Task<List<GetUsersByParamResult>> Handle(GetUsersByParamQuery request, CancellationToken cancellationToken)
        {
            var param = request.Param?.Trim().ToLower();


            var users = await this._userRepository.GetUsersByParamAsync(param ?? string.Empty);

            return users.Select(user => new GetUsersByParamResult
            {
                Route = user.Route,
                UserName = $"{user.Names} {user.PaternalSurName} {user.MaternalSurName}".Trim(),
            }).ToList();
        }
    }
}
