namespace BackEndAje.Api.Application.Dtos.Roles
{
    using BackEndAje.Api.Application.Behaviors;

    public class CreateRoleDto : IHasAuditInfo
    {
        public string RoleName { get; set; }

        public string Description { get; set; }

        public int CreatedBy { get; set; }

        public int UpdatedBy { get; set; }
    }
}
