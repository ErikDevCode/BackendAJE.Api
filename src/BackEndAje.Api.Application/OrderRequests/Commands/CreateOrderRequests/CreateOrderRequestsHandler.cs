namespace BackEndAje.Api.Application.OrderRequests.Commands.CreateOrderRequests
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class CreateOrderRequestsHandler : IRequestHandler<CreateOrderRequestsCommand, Unit>
    {
        private readonly IOrderRequestRepository _orderRequestRepository;
        private readonly IMapper _mapper;

        public CreateOrderRequestsHandler(IOrderRequestRepository orderRequestRepository, IMapper mapper)
        {
            this._orderRequestRepository = orderRequestRepository;
            this._mapper = mapper;
        }

        public async Task<Unit> Handle(CreateOrderRequestsCommand request, CancellationToken cancellationToken)
        {
            var orderRequest = this._mapper.Map<OrderRequest>(request);
            await this._orderRequestRepository.AddOrderRequestAsync(orderRequest);

            var orderRequestId = orderRequest.OrderRequestId;

            if (request.Documents == null || !request.Documents.Any())
            {
                return Unit.Value;
            }

            var orderRequestDocuments = this._mapper.Map<List<OrderRequestDocument>>(request.Documents);

            foreach (var document in orderRequestDocuments)
            {
                document.OrderRequestId = orderRequestId;
                document.IsActive = true;
                document.CreatedBy = request.CreatedBy;
                document.UpdatedBy = request.UpdatedBy;
                await this._orderRequestRepository.AddOrderRequestDocumentAsync(document);
            }

            return Unit.Value;
        }
    }
}