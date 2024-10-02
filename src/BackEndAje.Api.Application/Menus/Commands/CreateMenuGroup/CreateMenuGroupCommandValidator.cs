namespace BackEndAje.Api.Application.Menus.Commands.CreateMenuGroup
{
    using FluentValidation;

    public class CreateMenuGroupCommandValidator : AbstractValidator<CreateMenuGroupCommand>
    {
        public CreateMenuGroupCommandValidator()
        {
            this.RuleFor(x => x.MenuGroup.GroupName)
                .NotEmpty().WithMessage("Group name is required.")
                .MaximumLength(50).WithMessage("Permission name must not exceed 80 characters.");

            this.RuleFor(x => x.MenuGroup.IsSeparator)
                .NotEmpty().WithMessage("Separator is required.");
        }
    }
}

