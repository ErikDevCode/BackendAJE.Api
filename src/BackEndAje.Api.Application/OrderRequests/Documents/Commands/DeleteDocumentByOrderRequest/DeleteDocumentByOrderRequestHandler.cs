namespace BackEndAje.Api.Application.OrderRequests.Documents.Commands.DeleteDocumentByOrderRequest
{
    using BackEndAje.Api.Application.Interfaces;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class DeleteDocumentByOrderRequestHandler : IRequestHandler<DeleteDocumentByOrderRequestCommand, Unit>
    {
        private readonly IOrderRequestRepository _orderRequestRepository;
        private readonly IS3Service _s3Service;

        public DeleteDocumentByOrderRequestHandler(IOrderRequestRepository orderRequestRepository, IS3Service s3Service)
        {
            this._orderRequestRepository = orderRequestRepository;
            this._s3Service = s3Service;
        }

        public async Task<Unit> Handle(DeleteDocumentByOrderRequestCommand request, CancellationToken cancellationToken)
        {
            var orderRequestDocument = await this._orderRequestRepository.GetOrderRequestDocumentById(request.orderRequestDocumentId);
            if (orderRequestDocument == null)
            {
                throw new InvalidOperationException($"Documento con ID '{request.orderRequestDocumentId}' no existe.");
            }

            var key = $"{orderRequestDocument.FileFolder}/{orderRequestDocument.OrderRequestId}/{orderRequestDocument.DocumentName}";
            await this._s3Service.DeleteFileAsync(key);
            await this._orderRequestRepository.DeleteOrderRequestDocumentAsync(orderRequestDocument);
            return Unit.Value;
        }
    }
}
