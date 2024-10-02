namespace BackEndAje.Api.Application.Roles.Commands.AssignPermissionToRole
{
    using FluentValidation;

    public class AssignPermissionToRoleCommandValidator : AbstractValidator<AssignPermissionToRoleCommand>
    {
        public AssignPermissionToRoleCommandValidator()
        {
            this.RuleFor(x => x.AssignPermissionToRole.PermissionId)
                .NotEmpty().WithMessage("PermissionId is required.");
            this.RuleFor(x => x.AssignPermissionToRole.RoleId)
                .NotEmpty().WithMessage("RoleId is required.");
        }
    }
}
