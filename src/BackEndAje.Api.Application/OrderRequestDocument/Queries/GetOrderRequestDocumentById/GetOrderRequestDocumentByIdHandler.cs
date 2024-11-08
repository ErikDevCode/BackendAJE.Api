namespace BackEndAje.Api.Application.OrderRequestDocument.Queries.GetOrderRequestDocumentById
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetOrderRequestDocumentByIdHandler : IRequestHandler<GetOrderRequestDocumentByIdQuery, List<GetOrderRequestDocumentByIdResult>>
    {
        private readonly IOrderRequestRepository _orderRequestRepository;
        private readonly IMapper _mapper;

        public GetOrderRequestDocumentByIdHandler(IOrderRequestRepository orderRequestRepository, IMapper mapper)
        {
            this._orderRequestRepository = orderRequestRepository;
            this._mapper = mapper;
        }

        public async Task<List<GetOrderRequestDocumentByIdResult>> Handle(GetOrderRequestDocumentByIdQuery request, CancellationToken cancellationToken)
        {
            var documents = await this._orderRequestRepository.GetOrderRequestDocumentByOrderRequestId(request.orderRequestId);
            if (documents == null || !documents.Any())
            {
                return new List<GetOrderRequestDocumentByIdResult>();
            }

            var documentResults = documents.Select(doc =>
            {
                var result = this._mapper.Map<GetOrderRequestDocumentByIdResult>(doc);
                result.ContentType = doc.ContentType;
                result.FileName = doc.DocumentName;
                return result;
            }).ToList();

            return documentResults;
        }
    }
}
