namespace BackEndAje.Api.Application.Users.Commands.UpdateUser
{
    using BackEndAje.Api.Application.Dtos.Users;
    using MediatR;

    public class UpdateUserCommand : IRequest<UpdateUserResult>
    {
        public UpdateUserDto User { get; set; }
    }
}
