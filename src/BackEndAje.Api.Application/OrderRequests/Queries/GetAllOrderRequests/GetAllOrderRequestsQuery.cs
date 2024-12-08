namespace BackEndAje.Api.Application.OrderRequests.Queries.GetAllOrderRequests
{
    using BackEndAje.Api.Application.Abstractions.Common;
    using BackEndAje.Api.Application.Behaviors;
    using MediatR;

    public record GetAllOrderRequestsQuery(
        int? PageNumber = null,
        int? PageSize = null,
        int? ClientCode = null,
        int? OrderStatusId = null,
        int? ReasonRequestId = null,
        int? CediId = null,
        int? RegionId = null,
        DateTime? StartDate = null,
        DateTime? EndDate = null) : IRequest<PaginatedResult<GetAllOrderRequestsResult>>, IHasUserId
    {
        public int UserId { get; set; }
    }
}