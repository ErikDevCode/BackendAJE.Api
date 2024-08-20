namespace BackEndAje.Api.Application.Roles.Queries.GetAllRoles
{
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetAllRolesHandler : IRequestHandler<GetAllRolesQuery, List<GetAllRolesResult>>
    {
        private readonly IRoleRepository _roleRepository;

        public GetAllRolesHandler(IRoleRepository roleRepository)
        {
            this._roleRepository = roleRepository;
        }

        public async Task<List<GetAllRolesResult>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
        {
            var roles = await this._roleRepository.GetAllRolesAsync();
            var result = roles.Select(role => new GetAllRolesResult
            {
                RoleId = role.RoleId,
                RoleName = role.RoleName,
                Description = role.Description,
            }).ToList();

            return result;
        }
    }
}
