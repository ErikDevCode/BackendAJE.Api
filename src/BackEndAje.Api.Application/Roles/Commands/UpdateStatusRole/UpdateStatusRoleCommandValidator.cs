namespace BackEndAje.Api.Application.Roles.Commands.UpdateStatusRole
{
    using FluentValidation;

    public class UpdateStatusRoleCommandValidator : AbstractValidator<UpdateStatusRoleCommand>
    {
        public UpdateStatusRoleCommandValidator()
        {
            this.RuleFor(x => x.RoleUpdateStatus.RoleId)
                .NotEmpty().WithMessage("RoleId is required.");
        }
    }
}
