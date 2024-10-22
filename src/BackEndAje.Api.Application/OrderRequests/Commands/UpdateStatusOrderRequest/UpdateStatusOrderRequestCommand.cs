namespace BackEndAje.Api.Application.OrderRequests.Commands.UpdateStatusOrderRequest
{
    using MediatR;

    public class UpdateStatusOrderRequestCommand : IRequest<Unit>
    {
        public int OrderRequestId { get; set; }

        public int OrderStatusId { get; set; }

        public string ChangeReason { get; set; }

        public int CreatedBy { get; set; }
    }
}