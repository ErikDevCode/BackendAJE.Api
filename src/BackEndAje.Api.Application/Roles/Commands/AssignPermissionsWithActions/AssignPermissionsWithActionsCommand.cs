namespace BackEndAje.Api.Application.Roles.Commands.AssignPermissionsWithActions
{
    using BackEndAje.Api.Application.Behaviors;
    using MediatR;

    public class AssignPermissionsWithActionsCommand : IRequest<Unit>, IHasAuditInfo
    {
        public int RoleId { get; set; }

        public int PermissionId { get; set; }

        public int ActionId { get; set; }

        public bool Status { get; set; }

        public int CreatedBy { get; set; }

        public int UpdatedBy { get; set; }
    }
}
