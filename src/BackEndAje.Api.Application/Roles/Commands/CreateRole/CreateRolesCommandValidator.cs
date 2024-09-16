namespace BackEndAje.Api.Application.Roles.Commands.CreateRole
{
    using FluentValidation;

    public class CreateRolesCommandValidator : AbstractValidator<CreateRolesCommand>
    {
        public CreateRolesCommandValidator()
        {
            this.RuleFor(x => x.Role.RoleName)
                .NotEmpty().WithMessage("Role name is required.")
                .MaximumLength(50).WithMessage("Role name must not exceed 50 characters.");

            this.RuleFor(x => x.Role.Description)
                .MaximumLength(255).WithMessage("Description must not exceed 255 characters.");
        }
    }
}
