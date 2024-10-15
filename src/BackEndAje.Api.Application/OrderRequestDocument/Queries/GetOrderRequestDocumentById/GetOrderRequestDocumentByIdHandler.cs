namespace BackEndAje.Api.Application.OrderRequestDocument.Queries.GetOrderRequestDocumentById
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetOrderRequestDocumentByIdHandler : IRequestHandler<GetOrderRequestDocumentByIdQuery, GetOrderRequestDocumentByIdResult>
    {
        private readonly IOrderRequestRepository _orderRequestRepository;
        private readonly IMapper _mapper;

        public GetOrderRequestDocumentByIdHandler(IOrderRequestRepository orderRequestRepository, IMapper mapper)
        {
            this._orderRequestRepository = orderRequestRepository;
            this._mapper = mapper;
        }

        public async Task<GetOrderRequestDocumentByIdResult> Handle(GetOrderRequestDocumentByIdQuery request, CancellationToken cancellationToken)
        {
            var orderRequest = await this._orderRequestRepository.GetOrderRequestDocumentById(request.documentId);
            var contentType = this.GetContentType(orderRequest.DocumentName);
            var result = this._mapper.Map<GetOrderRequestDocumentByIdResult>(orderRequest);
            result.ContentType = contentType;
            result.FileName = orderRequest.DocumentName;
            return result;
        }

        private string GetContentType(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            return extension switch
            {
                ".pdf" => "application/pdf",
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".doc" => "application/msword",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                ".xls" => "application/vnd.ms-excel",
                ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                _ => "application/octet-stream",
            };
        }
    }
}
