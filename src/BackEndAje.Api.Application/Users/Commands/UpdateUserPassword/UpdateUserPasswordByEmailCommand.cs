namespace BackEndAje.Api.Application.Users.Commands.UpdateUserPassword
{
    using MediatR;

    public record UpdateUserPasswordByEmailCommand(string Email, string NewPassword) : IRequest<bool>;
}