namespace BackEndAje.Api.Application.Users.Commands.AssignRolesToUser
{
    using MediatR;

    public record AssignRolesToUserCommand(int UserId, List<string> Roles) : IRequest<AssingRolesToUserResult>;
}
