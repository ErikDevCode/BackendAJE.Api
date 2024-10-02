namespace BackEndAje.Api.Application.Dtos.Roles
{
    public class AssignPermissionToRoleDto
    {
        public int RoleId { get; set; }

        public int PermissionId { get; set; }

        public int CreatedBy { get; set; }

        public int UpdatedBy { get; set; }
    }
}
