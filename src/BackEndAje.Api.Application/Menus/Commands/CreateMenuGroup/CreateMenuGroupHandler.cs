namespace BackEndAje.Api.Application.Menus.Commands.CreateMenuGroup
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class CreateMenuGroupHandler : IRequestHandler<CreateMenuGroupCommand, Unit>
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IMapper _mapper;

        public CreateMenuGroupHandler(IMenuRepository menuRepository, IMapper mapper)
        {
            this._menuRepository = menuRepository;
            this._mapper = mapper;
        }

        public async Task<Unit> Handle(CreateMenuGroupCommand request, CancellationToken cancellationToken)
        {
            var listMenuGroup = await this._menuRepository.GetAllMenuGroupAsync();
            var existingMenuGroup = listMenuGroup.FirstOrDefault(r => r.GroupName.Equals(request.MenuGroup.GroupName, StringComparison.OrdinalIgnoreCase));

            if (existingMenuGroup != null)
            {
                throw new InvalidOperationException($"MenuGroup '{request.MenuGroup.GroupName}' already exists.");
            }

            var newMenuGroup = this._mapper.Map<MenuGroup>(request.MenuGroup);
            await this._menuRepository.AddMenuGroupAsync(newMenuGroup);
            return Unit.Value;
        }
    }
}
