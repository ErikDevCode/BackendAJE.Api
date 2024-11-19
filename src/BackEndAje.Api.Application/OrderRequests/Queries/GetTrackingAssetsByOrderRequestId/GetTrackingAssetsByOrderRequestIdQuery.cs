namespace BackEndAje.Api.Application.OrderRequests.Queries.GetTrackingAssetsByOrderRequestId
{
    using MediatR;

    public record GetTrackingAssetsByOrderRequestIdQuery(int orderRequestId) : IRequest<List<GetTrackingAssetsByOrderRequestIdResult>>;
}
