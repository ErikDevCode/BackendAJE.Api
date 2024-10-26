namespace BackEndAje.Api.Application.OrderRequests.Queries.GetAllOrderRequests
{
    using BackEndAje.Api.Application.Abstractions.Common;
    using MediatR;

    public record GetAllOrderRequestsQuery(
        int userId,
        int PageNumber = 1,
        int PageSize = 10,
        int? ClientCode = null,
        int? OrderStatusId = null,
        int? ReasonRequestId = null,
        DateTime? StartDate = null,
        DateTime? EndDate = null) : IRequest<PaginatedResult<GetAllOrderRequestsResult>>;
}