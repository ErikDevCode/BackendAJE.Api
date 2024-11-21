namespace BackEndAje.Api.Application.OrderRequests.Commands.UpdateStatusOrderRequest
{
    using BackEndAje.Api.Application.Behaviors;
    using MediatR;

    public class UpdateStatusOrderRequestCommand : IRequest<Unit>, IHasCreatedByInfo
    {
        public int OrderRequestId { get; set; }

        public int OrderStatusId { get; set; }

        public string ChangeReason { get; set; }

        public DateTime? StatusDate { get; set; }

        public int CreatedBy { get; set; }
    }
}