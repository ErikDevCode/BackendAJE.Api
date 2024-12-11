namespace BackEndAje.Api.Application.OrderRequests.Commands.DeleteOrderRequest
{
    using BackEndAje.Api.Application.Behaviors;
    using MediatR;

    public class DeleteOrderRequestCommand : IRequest<Unit>, IHasUpdatedByInfo
    {
        public int OrderRequestId { get; set; }

        public int UpdatedBy { get; set; }
    }
}

