namespace BackEndAje.Api.Application.Dtos.Roles
{
    public class UpdateRoleDto
    {
        public int RoleId { get; set; }

        public string RoleName { get; set; }

        public string Description { get; set; }

        public int UpdatedBy { get; set; }
    }
}
