namespace BackEndAje.Api.Application.Actions.Commands.DeleteAction
{
    using FluentValidation;

    public class DeleteActionCommandValidator : AbstractValidator<DeleteActionCommand>
    {
        public DeleteActionCommandValidator()
        {
            this.RuleFor(x => x.DeleteAction.ActionId)
                .NotEmpty().WithMessage("ActionId is required.");
        }
    }
}
