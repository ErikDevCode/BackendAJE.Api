namespace BackEndAje.Api.Application.OrderRequests.Queries.GetAllOrderRequests
{
    using BackEndAje.Api.Application.Abstractions.Common;
    using MediatR;

    public record GetAllOrderRequestsQuery(int PageNumber = 1, int PageSize = 10) : IRequest<PaginatedResult<GetAllOrderRequestsResult>>;
}