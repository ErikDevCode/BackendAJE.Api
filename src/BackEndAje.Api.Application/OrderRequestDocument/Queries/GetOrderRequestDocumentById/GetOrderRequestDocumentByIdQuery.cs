namespace BackEndAje.Api.Application.OrderRequestDocument.Queries.GetOrderRequestDocumentById
{
    using MediatR;

    public record GetOrderRequestDocumentByIdQuery(int orderRequestId) : IRequest<List<GetOrderRequestDocumentByIdResult>>;
}
