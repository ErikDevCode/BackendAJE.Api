namespace BackEndAje.Api.Application.Dtos.Roles
{
    using BackEndAje.Api.Application.Behaviors;

    public class UpdateRoleDto : IHasUpdatedByInfo
    {
        public int RoleId { get; set; }

        public string RoleName { get; set; }

        public string Description { get; set; }

        public int UpdatedBy { get; set; }
    }
}
