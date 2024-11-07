namespace BackEndAje.Api.Application.Roles.Queries.GetAllRolesWithPermissions
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetAllRolesWithPermissionsHandler : IRequestHandler<GetAllRolesWithPermissionsQuery, List<GetAllRolesWithPermissionsResult>>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public GetAllRolesWithPermissionsHandler(IRoleRepository roleRepository, IMapper mapper)
        {
            this._roleRepository = roleRepository;
            this._mapper = mapper;
        }

        public async Task<List<GetAllRolesWithPermissionsResult>> Handle(GetAllRolesWithPermissionsQuery request, CancellationToken cancellationToken)
        {
            var rolesWithPermissions = await this._roleRepository.GetRoleWithPermissionsAsync(request.roleId);

            return this._mapper.Map<List<GetAllRolesWithPermissionsResult>>(rolesWithPermissions);
        }
    }
}
