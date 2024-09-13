namespace BackEndAje.Api.Application.Users.Commands.AssignRolesToUser
{
    using MediatR;

    public record AssignRolesToUserCommand(int UserId, int RoleId) : IRequest<AssingRolesToUserResult>;
}
