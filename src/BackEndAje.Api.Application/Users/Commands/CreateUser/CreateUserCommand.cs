namespace BackEndAje.Api.Application.Users.Commands.CreateUser
{
    using BackEndAje.Api.Application.Dtos.Users;
    using MediatR;

    public class CreateUserCommand : IRequest<CreateUserResult>
    {
        public CreateUserDto User { get; set; }
    }
}
