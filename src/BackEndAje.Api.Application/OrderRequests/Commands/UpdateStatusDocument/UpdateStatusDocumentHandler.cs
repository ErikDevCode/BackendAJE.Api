namespace BackEndAje.Api.Application.OrderRequests.Commands.UpdateStatusDocument
{
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class UpdateStatusDocumentHandler : IRequestHandler<UpdateStatusDocumentCommand, Unit>
    {
        private readonly IOrderRequestRepository _orderRequestRepository;

        public UpdateStatusDocumentHandler(IOrderRequestRepository orderRequestRepository)
        {
            this._orderRequestRepository = orderRequestRepository;
        }

        public async Task<Unit> Handle(UpdateStatusDocumentCommand request, CancellationToken cancellationToken)
        {
            var existingDocument = await this._orderRequestRepository.GetOrderRequestDocumentById(request.DocumentId);
            if (existingDocument == null)
            {
                throw new InvalidOperationException($"Document with Id '{request.DocumentId}' not exists.");
            }

            existingDocument.IsActive = existingDocument.IsActive is false;
            existingDocument.UpdatedBy = request.UpdatedBy;
            await this._orderRequestRepository.UpdateStatusOrderRequestDocumentAsync(existingDocument);
            return Unit.Value;
        }
    }
}