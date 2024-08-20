namespace BackEndAje.Api.Application.Roles.Commands.CreateRole
{
    using FluentValidation;

    public class CreateRolesCommandValidator : AbstractValidator<CreateRolesCommand>
    {
        public CreateRolesCommandValidator()
        {
            this.RuleFor(x => x.RoleName)
                .NotEmpty().WithMessage("Role name is required.")
                .MaximumLength(10).WithMessage("Role name must not exceed 10 characters.");

            this.RuleFor(x => x.Description)
                .MaximumLength(255).WithMessage("Description must not exceed 255 characters.");
        }
    }
}
