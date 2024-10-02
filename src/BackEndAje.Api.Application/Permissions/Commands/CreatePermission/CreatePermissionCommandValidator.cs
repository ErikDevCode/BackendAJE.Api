namespace BackEndAje.Api.Application.Permissions.Commands.CreatePermission
{
    using FluentValidation;

    public class CreatePermissionCommandValidator : AbstractValidator<CreatePermissionCommand>
    {
        public CreatePermissionCommandValidator()
        {
            this.RuleFor(x => x.Permission.PermissionName)
                .NotEmpty().WithMessage("Permission name is required.")
                .MaximumLength(80).WithMessage("Permission name must not exceed 80 characters.");
        }
    }
}
