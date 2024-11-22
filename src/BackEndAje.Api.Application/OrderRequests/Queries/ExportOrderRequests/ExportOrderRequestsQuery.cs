namespace BackEndAje.Api.Application.OrderRequests.Queries.ExportOrderRequests
{
    using BackEndAje.Api.Application.Behaviors;
    using MediatR;

    public record ExportOrderRequestsQuery(
        int? ClientCode = null,
        int? OrderStatusId = null,
        int? ReasonRequestId = null,
        DateTime? StartDate = null,
        DateTime? EndDate = null) : IRequest<byte[]>, IHasUserId
    {
        public int UserId { get; set; }
    }
}
