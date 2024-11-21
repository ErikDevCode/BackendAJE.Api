namespace BackEndAje.Api.Application.Users.Commands.AssignRolesToUser
{
    using BackEndAje.Api.Application.Behaviors;
    using MediatR;

    public class AssignRolesToUserCommand() : IRequest<AssingRolesToUserResult>, IHasAuditInfo
    {
        public int UserId { get; set; }

        public int RoleId { get; set; }

        public int CreatedBy { get; set; }

        public int UpdatedBy { get; set; }
    }
}
