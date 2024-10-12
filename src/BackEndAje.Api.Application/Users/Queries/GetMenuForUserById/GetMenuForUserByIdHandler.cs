namespace BackEndAje.Api.Application.Users.Queries.GetMenuForUserById
{
    using BackEndAje.Api.Application.Dtos.Users.Menu;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetMenuForUserByIdHandler : IRequestHandler<GetMenuForUserByIdQuery, GetMenuForUserByIdResult>
    {
        private readonly IUserRepository _userRepository;

        public GetMenuForUserByIdHandler(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }

        public async Task<GetMenuForUserByIdResult> Handle(GetMenuForUserByIdQuery request, CancellationToken cancellationToken)
        {
            var menuItems = await this._userRepository.GetMenuForUserByIdAsync(request.UserId);

            return this.MapToGetMenuForUserByIdResult(menuItems);
        }

        private GetMenuForUserByIdResult MapToGetMenuForUserByIdResult(List<MenuItem> menuItems)
        {
            return new GetMenuForUserByIdResult
            {
                label = "Módulos",
                Items = this.MapToMenuItems(menuItems),
            };
        }

        private List<MenuItemDto> MapToMenuItems(List<MenuItem> menuItems)
        {
            return menuItems
                .Where(mi => !mi.ParentMenuItemId.HasValue)
                .Select(this.MapToMenuItemDto)
                .ToList() ?? new List<MenuItemDto>();
        }

        private MenuItemDto MapToMenuItemDto(MenuItem mi)
        {
            var menuItemDto = new MenuItemDto
            {
                Label = mi.Label,
                Icon = mi.Icon,
                RouterLink = mi.Route,
                Permissions = this.MapToPermissions(mi.MenuItemActions),
            };

            return menuItemDto;
        }

        private List<string> MapToPermissions(ICollection<MenuItemAction>? menuItemActions)
        {
            return menuItemActions?
                .Where(mia => mia.RoleMenuAccesses.Count != 0)
                .SelectMany(mia => mia.RoleMenuAccesses.Select(rma => $"{rma.RolePermission.Permission.PermissionName}:{mia.Action.ActionName}"))
                .Distinct()
                .ToList() ?? new List<string>();
        }
    }
}
