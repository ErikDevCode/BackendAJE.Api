namespace BackEndAje.Api.Application.OrderRequests.Commands.UpdateStatusDocument
{
    using MediatR;

    public class UpdateStatusDocumentCommand : IRequest<Unit>
    {
        public int DocumentId { get; set; }

        public int UpdatedBy { get; set; }
    }
}