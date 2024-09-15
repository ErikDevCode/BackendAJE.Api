namespace BackEndAje.Api.Application.Users.Commands.AssignRolesToUser
{
    using MediatR;

    public class AssignRolesToUserCommand() : IRequest<AssingRolesToUserResult>
    {
        public int UserId { get; set; }

        public int RoleId { get; set; }

        public int CreatedBy { get; set; }

        public int UpdatedBy { get; set; }
    }
}
