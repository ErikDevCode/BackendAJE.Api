namespace BackEndAje.Api.Application.Clients.Commands.CreateClient
{
    using FluentValidation;

    public class CreateClientCommandValidator : AbstractValidator<CreateClientCommand>
    {
        public CreateClientCommandValidator()
        {
            this.RuleFor(x => x.ClientCode)
                .NotEmpty().WithMessage("Código Cliente es requerido.");

            this.RuleFor(x => x.Route)
                .NotEmpty().WithMessage("Ruta es requerido.");

            this.RuleFor(x => x.CompanyName)
                .NotEmpty().WithMessage("Razón Social es requerido.");

            this.RuleFor(x => x.DocumentNumber)
                .NotEmpty().WithMessage("Número de documento es requerido.");
        }
    }
}

