namespace BackEndAje.Api.Application.Roles.Queries.GetAllRoles
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetAllRolesHandler : IRequestHandler<GetAllRolesQuery, List<GetAllRolesResult>>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public GetAllRolesHandler(IRoleRepository roleRepository, IMapper mapper)
        {
            this._roleRepository = roleRepository;
            this._mapper = mapper;
        }

        public async Task<List<GetAllRolesResult>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
        {
            var roles = await this._roleRepository.GetAllRolesAsync();
            var result = this._mapper.Map<List<GetAllRolesResult>>(roles);
            return result;
        }
    }
}
