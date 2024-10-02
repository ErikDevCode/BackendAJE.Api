namespace BackEndAje.Api.Application.Menus.Queries.GetAllMenuGroups
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetAllMenuGroupsHandler : IRequestHandler<GetAllMenuGroupsQuery, List<GetAllMenuGroupsResult>>
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IMapper _mapper;

        public GetAllMenuGroupsHandler(IMenuRepository menuRepository, IMapper mapper)
        {
            this._menuRepository = menuRepository;
            this._mapper = mapper;
        }

        public async Task<List<GetAllMenuGroupsResult>> Handle(GetAllMenuGroupsQuery request, CancellationToken cancellationToken)
        {
            var menuGroup = await this._menuRepository.GetAllMenuGroupAsync();
            var result = this._mapper.Map<List<GetAllMenuGroupsResult>>(menuGroup);
            return result;
        }
    }
}
