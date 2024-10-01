namespace BackEndAje.Api.Application.Actions.Commands.CreateAction
{
    using FluentValidation;

    public class CreateActionCommandValidator : AbstractValidator<CreateActionCommand>
    {
        public CreateActionCommandValidator()
        {
            this.RuleFor(x => x.Action.ActionName)
                .NotEmpty().WithMessage("Action name is required.")
                .MaximumLength(80).WithMessage("Action name must not exceed 80 characters.");
        }
    }
}
