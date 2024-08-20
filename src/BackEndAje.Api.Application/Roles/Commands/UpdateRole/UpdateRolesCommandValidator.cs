namespace BackEndAje.Api.Application.Roles.Commands.UpdateRole
{
    using FluentValidation;

    public class UpdateRolesCommandValidator : AbstractValidator<UpdateRolesCommand>
    {
        public UpdateRolesCommandValidator()
        {
            this.RuleFor(x => x.RoleId)
                .NotEmpty().WithMessage("Role name is required.");
            this.RuleFor(x => x.RoleName)
                .NotEmpty().WithMessage("Role name is required.")
                .MaximumLength(50).WithMessage("Role name must not exceed 50 characters.");

            this.RuleFor(x => x.Description)
                .MaximumLength(255).WithMessage("Description must not exceed 255 characters.");
        }
    }
}
