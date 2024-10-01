namespace BackEndAje.Api.Application.Actions.Commands.UpdateAction
{
    using FluentValidation;

    public class UpdateActionCommandValidator : AbstractValidator<UpdateActionCommand>
    {
        public UpdateActionCommandValidator()
        {
            this.RuleFor(x => x.Action.ActionId)
                .NotEmpty().WithMessage("ActionId is required.");

            this.RuleFor(x => x.Action.ActionName)
                .NotEmpty().WithMessage("Action name is required.")
                .MaximumLength(80).WithMessage("Action name must not exceed 80 characters.");
        }
    }
}
