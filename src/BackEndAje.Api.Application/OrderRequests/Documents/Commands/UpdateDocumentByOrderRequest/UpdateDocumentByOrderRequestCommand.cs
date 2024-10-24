namespace BackEndAje.Api.Application.OrderRequests.Documents.Commands.UpdateDocumentByOrderRequest
{
    using MediatR;

    public class UpdateDocumentByOrderRequestCommand : IRequest<Unit>
    {
        public int DocumentId { get; set; }

        public int OrderRequestId { get; set; }

        public string DocumentName { get; set; }

        public decimal DocumentWeight { get; set; }

        public byte[] DocumentContent { get; set; }

        public bool IsActive { get; set; }

        public int UpdatedBy { get; set; }
    }
}
