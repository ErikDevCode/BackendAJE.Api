namespace BackEndAje.Api.Application.OrderRequests.Documents.Commands.CreateDocumentByOrderRequest
{
    using BackEndAje.Api.Application.Interfaces;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class CreateDocumentByOrderRequestHandler : IRequestHandler<CreateDocumentByOrderRequestCommand, Unit>
    {
        private readonly IOrderRequestRepository _orderRequestRepository;
        private readonly IS3Service _s3Service;

        public CreateDocumentByOrderRequestHandler(IOrderRequestRepository orderRequestRepository, IS3Service s3Service)
        {
            this._orderRequestRepository = orderRequestRepository;
            this._s3Service = s3Service;
        }

        public async Task<Unit> Handle(CreateDocumentByOrderRequestCommand request, CancellationToken cancellationToken)
        {
            if (request.DocumentFile == null || request.DocumentFile.Length == 0)
            {
                throw new InvalidOperationException("El archivo del documento es requerido.");
            }

            var documentName = request.DocumentFile.FileName;
            var documentWeight = (decimal)request.DocumentFile.Length / (1024 * 1024); // En MB
            var contentType = request.DocumentFile.ContentType;
            const string fileFolder = "order-request-documents";
            var fileName = $"{documentName}";

            // Subir archivo a S3
            using var memoryStream = new MemoryStream();
            await request.DocumentFile.CopyToAsync(memoryStream, cancellationToken);
            var s3Url = await this._s3Service.UploadFileAsync(memoryStream, fileFolder, request.OrderRequestId.ToString(), fileName);

            var document = new OrderRequestDocument
            {
                OrderRequestId = request.OrderRequestId,
                Url = s3Url,
                DocumentName = documentName,
                DocumentWeight = documentWeight,
                ContentType = contentType,
                FileFolder = fileFolder,
                IsActive = true,
                CreatedBy = request.CreatedBy,
                UpdatedBy = request.UpdatedBy,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            await this._orderRequestRepository.AddOrderRequestDocumentAsync(document);

            return Unit.Value;
        }
    }
}