namespace BackEndAje.Api.Application.Roles.Queries.GetPermissionsWithActionByRoleId
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetPermissionsWithActionByRoleIdHandler : IRequestHandler<GetPermissionsWithActionByRoleIdQuery, List<GetPermissionsWithActionByRoleIdResult>>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public GetPermissionsWithActionByRoleIdHandler(IRoleRepository roleRepository, IMapper mapper)
        {
            this._roleRepository = roleRepository;
            this._mapper = mapper;
        }

        public async Task<List<GetPermissionsWithActionByRoleIdResult>> Handle(GetPermissionsWithActionByRoleIdQuery request, CancellationToken cancellationToken)
        {
            var permissionsWithActions = await this._roleRepository.GetPermissionsWithActionByRoleIdAsync(request.RoleId);
            return this._mapper.Map<List<GetPermissionsWithActionByRoleIdResult>>(permissionsWithActions);
        }
    }
}

