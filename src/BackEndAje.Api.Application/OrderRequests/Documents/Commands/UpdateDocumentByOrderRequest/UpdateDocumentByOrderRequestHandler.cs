namespace BackEndAje.Api.Application.OrderRequests.Documents.Commands.UpdateDocumentByOrderRequest
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class UpdateDocumentByOrderRequestHandler : IRequestHandler<UpdateDocumentByOrderRequestCommand, Unit>
    {
        private readonly IOrderRequestRepository _orderRequestRepository;
        private readonly IMapper _mapper;

        public UpdateDocumentByOrderRequestHandler(IOrderRequestRepository orderRequestRepository, IMapper mapper)
        {
            this._orderRequestRepository = orderRequestRepository;
            this._mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateDocumentByOrderRequestCommand request, CancellationToken cancellationToken)
        {
            var orderRequestDocument = await this._orderRequestRepository.GetOrderRequestDocumentById(request.DocumentId);
            if (orderRequestDocument == null)
            {
                throw new InvalidOperationException($"DocumentId '{request.DocumentId}' not exists.");
            }

            this._mapper.Map(request, orderRequestDocument);
            await this._orderRequestRepository.UpdateOrderRequestDocumentAsync(orderRequestDocument);
            return Unit.Value;
        }
    }
}
