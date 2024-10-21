namespace BackEndAje.Api.Application.Users.Queries.GetAllUser
{
    using AutoMapper;
    using BackEndAje.Api.Application.Abstractions.Common;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetAllUserHandler : IRequestHandler<GetAllUserQuery, PaginatedResult<GetAllUserResult>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetAllUserHandler(IUserRepository userRepository, IMapper mapper)
        {
            this._userRepository = userRepository;
            this._mapper = mapper;
        }

        public async Task<PaginatedResult<GetAllUserResult>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
        {
            var users = await this._userRepository.GetAllUsers(request.PageNumber, request.PageSize);
            var result = this._mapper.Map<List<GetAllUserResult>>(users);
            var totalUsers = await this._userRepository.GetTotalUsers();

            var paginatedResult = new PaginatedResult<GetAllUserResult>
            {
                TotalCount = totalUsers,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Items = result,
            };

            return paginatedResult;
        }
    }
}