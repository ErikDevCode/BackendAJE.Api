using FluentValidation;

namespace BackEndAje.Api.Application.OrderRequests.Commands.CreateOrderRequests
{
    public class CreateOrderRequestscommandValidator : AbstractValidator<CreateOrderRequestsCommand>
    {
        public CreateOrderRequestscommandValidator()
        {
            this.RuleFor(x => x.SupervisorId)
                .NotEmpty().WithMessage("supervisorId is required.");
        }
    }
}