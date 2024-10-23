namespace BackEndAje.Api.Application.Users.Queries.GetSupervisorByCedi
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetSupervisorByCediHandler : IRequestHandler<GetSupervisorByCediQuery, List<GetSupervisorByCediResult>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetSupervisorByCediHandler(IUserRepository userRepository, IMapper mapper)
        {
            this._userRepository = userRepository;
            this._mapper = mapper;
        }

        public async Task<List<GetSupervisorByCediResult>> Handle(GetSupervisorByCediQuery request, CancellationToken cancellationToken)
        {
            var users = await this._userRepository.GetSupervisorByCediId(request.CediId);
            return this._mapper.Map<List<GetSupervisorByCediResult>>(users);
        }
    }
}