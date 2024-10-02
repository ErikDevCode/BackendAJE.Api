namespace BackEndAje.Api.Application.Permissions.Commands.DeletePermission
{
    using FluentValidation;

    public class DeletePermissionCommandValidator : AbstractValidator<DeletePermissionCommand>
    {
        public DeletePermissionCommandValidator()
        {
            this.RuleFor(x => x.DeletePermission.PermissionId)
                .NotEmpty().WithMessage("PermissionId is required.");
        }
    }
}
