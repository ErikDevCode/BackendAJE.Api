using BackEndAje.Api.Domain.Repositories;
using MediatR;

namespace BackEndAje.Api.Application.Users.Queries.GetUsersByParam
{
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


            var users = await this._userRepository.GetUsersByParamAsync(param);

            return users.Select(user => new GetUsersByParamResult
            {
                Route = user.Route,
                UserName = $"{user.Names} {user.PaternalSurName} {user.MaternalSurName}".Trim(),
            }).ToList();
        }
    }
}
