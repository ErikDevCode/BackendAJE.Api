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
            var menuGroups = await this._userRepository.GetMenuForUserByIdAsync(request.UserId);

            return this.MapToGetMenuForUserByIdResult(menuGroups);
        }

        private GetMenuForUserByIdResult MapToGetMenuForUserByIdResult(List<MenuGroup> menuGroups)
        {
            return new GetMenuForUserByIdResult
            {
                MenuGroups = menuGroups.Select(this.MapToMenuGroupDto).ToList(),
            };
        }

        private MenuGroupDto MapToMenuGroupDto(MenuGroup mg)
        {
            return new MenuGroupDto
            {
                Group = mg.GroupName,
                Separator = mg.IsSeparator,
                Items = this.MapToMenuItems(mg),
            };
        }

        private List<MenuItemDto> MapToMenuItems(MenuGroup mg)
        {
            return mg.MenuItems?
                .Where(mi => !mg.MenuItems.Any(parent => parent.ChildItems.Any(child => child.MenuItemId == mi.MenuItemId)))
                .Select(this.MapToMenuItemDto)
                .ToList() ?? new List<MenuItemDto>();
        }

        private MenuItemDto MapToMenuItemDto(MenuItem mi)
        {
            var menuItemDto = new MenuItemDto
            {
                Label = mi.Label,
                Icon = mi.Icon,
                Route = mi.Route,
                Permissions = this.MapToPermissions(mi.MenuItemActions),
                Children = mi.ChildItems.Any() ? this.MapToChildren(mi.ChildItems) : null,
            };

            if (menuItemDto.Children == null)
            {
                menuItemDto.GetType().GetProperty(nameof(menuItemDto.Children))?.SetValue(menuItemDto, null);
            }

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

        private List<MenuItemDto> MapToChildren(ICollection<MenuItem>? childMenuChildren)
        {
            return childMenuChildren?
                .Select(mc => new MenuItemDto
                {
                    Label = mc.Label,
                    Route = mc.Route,
                    Icon = mc.Icon,
                    Permissions = this.MapToPermissions(mc.MenuItemActions),
                }).ToList() ?? new List<MenuItemDto>();
        }
    }
}
