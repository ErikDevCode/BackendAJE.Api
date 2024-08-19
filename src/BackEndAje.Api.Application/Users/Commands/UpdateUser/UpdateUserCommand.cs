namespace BackEndAje.Api.Application.Users.Commands.UpdateUser
{
    using MediatR;

    public record UpdateUserCommand(string Username, string Email) : IRequest<UpdateUserResult>;
}
