namespace BackEndAje.Api.Application.Clients.Commands.DisableClient
{
    using FluentValidation;

    public class DisableClientCommandValidator : AbstractValidator<DisableClientCommand>
    {
        public DisableClientCommandValidator()
        {
            this.RuleFor(x => x.ClientCode)
                .NotEmpty().WithMessage("Código Cliente es requerido.");
        }
    }
}
