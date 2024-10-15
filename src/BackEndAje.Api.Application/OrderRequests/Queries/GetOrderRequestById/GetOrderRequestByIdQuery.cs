namespace BackEndAje.Api.Application.OrderRequests.Queries.GetOrderRequestById
{
    using MediatR;

    public record GetOrderRequestByIdQuery(int orderRequestId) : IRequest<GetOrderRequestByIdResult>;
}
