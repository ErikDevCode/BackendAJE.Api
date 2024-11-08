namespace BackEndAje.Api.Application.OrderRequests.Documents.Commands.DeleteDocumentByOrderRequest
{
    using MediatR;

    public class DeleteDocumentByOrderRequestCommand : IRequest<Unit>
    {
        public int orderRequestDocumentId { get; set; }
    }
}
