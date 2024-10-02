namespace BackEndAje.Api.Application.Menus.Commands.CreateMenuGroup
{
    using BackEndAje.Api.Application.Dtos.Menu;
    using MediatR;

    public class CreateMenuGroupCommand : IRequest<Unit>
    {
        public CreateMenuGroupDto MenuGroup { get; set; }
    }
}
