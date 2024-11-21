namespace BackEndAje.Api.Application.Dtos.Roles
{
    using BackEndAje.Api.Application.Behaviors;

    public class UpdateStatusRoleDto : IHasUpdatedByInfo
    {
         public int RoleId { get; set; }

         public int UpdatedBy { get; set; }
    }
}
