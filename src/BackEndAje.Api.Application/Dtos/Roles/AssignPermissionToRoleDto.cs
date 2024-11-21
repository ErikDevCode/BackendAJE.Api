namespace BackEndAje.Api.Application.Dtos.Roles
{
    using BackEndAje.Api.Application.Behaviors;

    public class AssignPermissionToRoleDto : IHasAuditInfo
    {
        public int RoleId { get; set; }

        public int PermissionId { get; set; }

        public bool Status { get; set; }

        public int CreatedBy { get; set; }

        public int UpdatedBy { get; set; }
    }
}
