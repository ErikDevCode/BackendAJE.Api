namespace BackEndAje.Api.Application.Users.Commands.UpdateUserPassword
{
    using MediatR;

    public record UpdateUserPasswordByEmailCommand(string RouteOrEmail, string NewPassword) : IRequest<bool>;
}