namespace BackEndAje.Api.Application.Roles.Queries.GetAllRoles
{
    using AutoMapper;
    using BackEndAje.Api.Application.Abstractions.Common;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetAllRolesHandler : IRequestHandler<GetAllRolesQuery, PaginatedResult<GetAllRolesResult>>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public GetAllRolesHandler(IRoleRepository roleRepository, IMapper mapper)
        {
            this._roleRepository = roleRepository;
            this._mapper = mapper;
        }

        public async Task<PaginatedResult<GetAllRolesResult>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
        {
            var roles = await this._roleRepository.GetAllPaginateRolesAsync(request.PageNumber, request.PageSize);
            var totalRoles = await this._roleRepository.GetTotalRolesCountAsync();
            var result = this._mapper.Map<List<GetAllRolesResult>>(roles);
            var paginatedResult = new PaginatedResult<GetAllRolesResult>
            {
                TotalCount = totalRoles,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Items = result,
            };

            return paginatedResult;
        }
    }
}
