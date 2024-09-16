namespace BackEndAje.Api.Application.Dtos.Roles
{
    public class CreateRoleDto
    {
        public string RoleName { get; set; }

        public string Description { get; set; }

        public int CreatedBy { get; set; }

        public int UpdatedBy { get; set; }
    }
}
