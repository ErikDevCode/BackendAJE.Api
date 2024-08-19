namespace BackEndAje.Api.Application.Users.Commands.CreateUser
{
    using MediatR;

    public record CreateUserCommand(string Username, string Email, string Password) : IRequest<CreateUserResult>;
}
