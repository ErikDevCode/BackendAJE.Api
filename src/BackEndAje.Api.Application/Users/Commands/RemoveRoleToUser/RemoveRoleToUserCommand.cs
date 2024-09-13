namespace BackEndAje.Api.Application.Users.Commands.RemoveRoleToUser
{
    using MediatR;

    public record RemoveRolesToUserCommand(int UserId, int RoleId) : IRequest<Unit>;
}