namespace BackEndAje.Api.Application.Dtos.Permissions
{
    public class UpdatePermissionDto
    {
        public int PermissionId { get; set; }

        public string PermissionName { get; set; }

        public int UpdatedBy { get; set; }
    }
}
