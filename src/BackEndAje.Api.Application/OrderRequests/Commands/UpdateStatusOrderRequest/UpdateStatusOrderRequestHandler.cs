using BackEndAje.Api.Domain.Repositories;
using MediatR;

namespace BackEndAje.Api.Application.OrderRequests.Commands.UpdateStatusOrderRequest
{
    public class UpdateStatusOrderRequestHandler : IRequestHandler<UpdateStatusOrderRequestCommand, Unit>
    {
        private readonly IOrderRequestRepository _orderRequestRepository;

        public UpdateStatusOrderRequestHandler(IOrderRequestRepository orderRequestRepository)
        {
            this._orderRequestRepository = orderRequestRepository;
        }

        public Task<Unit> Handle(UpdateStatusOrderRequestCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
