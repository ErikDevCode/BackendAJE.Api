namespace BackEndAje.Api.Application.OrderRequests.Queries.GetTrackingByOrderRequestId
{
    using MediatR;

    public record GetTrackingByOrderRequestIdQuery(int orderRequestId) : IRequest<List<GetTrackingByOrderRequestIdResult>>;
}

