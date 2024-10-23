namespace BackEndAje.Api.Application.Users.Queries.GetMenuForUserById
{
    using AutoMapper;
    using BackEndAje.Api.Application.Dtos.Users.Menu;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetMenuForUserByIdHandler : IRequestHandler<GetMenuForUserByIdQuery, GetMenuForUserByIdResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetMenuForUserByIdHandler(IUserRepository userRepository, IMapper mapper)
        {
            this._userRepository = userRepository;
            this._mapper = mapper;
        }

        public async Task<GetMenuForUserByIdResult> Handle(GetMenuForUserByIdQuery request, CancellationToken cancellationToken)
        {
            var menuItems = await this._userRepository.GetMenuForUserByIdAsync(request.UserId);

            var menuItemMap = this._mapper.Map<List<MenuItemDto>>(menuItems);
            return this.MapToGetMenuForUserByIdResult(menuItemMap);
        }

        private GetMenuForUserByIdResult MapToGetMenuForUserByIdResult(List<MenuItemDto> menuItems)
        {
            return new GetMenuForUserByIdResult
            {
                label = "Módulos",
                Items = menuItems,
            };
        }
    }
}
