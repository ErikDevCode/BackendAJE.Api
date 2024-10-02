namespace BackEndAje.Api.Application.Permissions.Queries.GetAllPermissions
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetAllPermissionsHandler : IRequestHandler<GetAllPermissionsQuery, List<GetAllPermissionsResult>>
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IMapper _mapper;

        public GetAllPermissionsHandler(IPermissionRepository permissionRepository, IMapper mapper)
        {
            this._permissionRepository = permissionRepository;
            this._mapper = mapper;
        }

        public async Task<List<GetAllPermissionsResult>> Handle(GetAllPermissionsQuery request, CancellationToken cancellationToken)
        {
            var permissions = await this._permissionRepository.GetAllPermissionsAsync();
            var result = this._mapper.Map<List<GetAllPermissionsResult>>(permissions);
            return result;
        }
    }
}
