namespace BackEndAje.Api.Application.OrderRequests.Documents.Commands.CreateDocumentByOrderRequest
{
    using MediatR;

    public class CreateDocumentByOrderRequestCommand : IRequest<Unit>
    {
        public int OrderRequestId { get; set; }

        public string DocumentName { get; set; }

        public decimal DocumentWeight { get; set; }

        public byte[] DocumentContent { get; set; }

        public bool IsActive { get; set; }

        public int CreatedBy { get; set; }

        public int UpdatedBy { get; set; }
    }
}
