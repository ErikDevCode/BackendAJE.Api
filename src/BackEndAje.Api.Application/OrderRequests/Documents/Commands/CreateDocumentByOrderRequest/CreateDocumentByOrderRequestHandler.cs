namespace BackEndAje.Api.Application.OrderRequests.Documents.Commands.CreateDocumentByOrderRequest
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class CreateDocumentByOrderRequestHandler : IRequestHandler<CreateDocumentByOrderRequestCommand, Unit>
    {
        private readonly IOrderRequestRepository _orderRequestRepository;
        private readonly IMapper _mapper;

        public CreateDocumentByOrderRequestHandler(IOrderRequestRepository orderRequestRepository, IMapper mapper)
        {
            this._orderRequestRepository = orderRequestRepository;
            this._mapper = mapper;
        }

        public async Task<Unit> Handle(CreateDocumentByOrderRequestCommand request, CancellationToken cancellationToken)
        {
            var document = this._mapper.Map<OrderRequestDocument>(request);
            document.IsActive = true;
            await this._orderRequestRepository.AddOrderRequestDocumentAsync(document);
            return Unit.Value;
        }
    }
}