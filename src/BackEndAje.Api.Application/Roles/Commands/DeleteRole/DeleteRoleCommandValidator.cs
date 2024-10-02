namespace BackEndAje.Api.Application.Roles.Commands.DeleteRole
{
    using FluentValidation;

    public class DeleteRoleCommandValidator : AbstractValidator<DeleteRoleCommand>
    {
        public DeleteRoleCommandValidator()
        {
            this.RuleFor(x => x.RoleDelete.RoleId)
                .NotEmpty().WithMessage("RoleId is required.");
        }
    }
}
