namespace BackEndAje.Api.Application.OrderRequests.Documents.Commands.CreateDocumentByOrderRequest
{
    using BackEndAje.Api.Application.Behaviors;
    using MediatR;
    using Microsoft.AspNetCore.Http;

    public class CreateDocumentByOrderRequestCommand : IRequest<Unit>, IHasAuditInfo
    {
        public int OrderRequestId { get; set; }

        public IFormFile DocumentFile { get; set; }

        public int CreatedBy { get; set; }

        public int UpdatedBy { get; set; }
    }
}
