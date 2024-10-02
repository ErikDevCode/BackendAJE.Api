namespace BackEndAje.Api.Application.Dtos.Permissions
{
    public class CreatePermissionDto
    {
        public string PermissionName { get; set; }

        public int CreatedBy { get; set; }

        public int UpdatedBy { get; set; }
    }
}
