namespace BackEndAje.Api.Application.OrderRequests.Queries.GetOrderRequestById
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetOrderRequestByIdHandler : IRequestHandler<GetOrderRequestByIdQuery, GetOrderRequestByIdResult>
    {
        private readonly IOrderRequestRepository _orderRequestRepository;
        private readonly IMapper _mapper;

        public GetOrderRequestByIdHandler(IOrderRequestRepository orderRequestRepository, IMapper mapper)
        {
            this._orderRequestRepository = orderRequestRepository;
            this._mapper = mapper;
        }

        public async Task<GetOrderRequestByIdResult> Handle(GetOrderRequestByIdQuery request, CancellationToken cancellationToken)
        {
            var orderRequest = await this._orderRequestRepository.GetOrderRequestById(request.orderRequestId);

            if (orderRequest == null)
            {
                throw new KeyNotFoundException($"Solicitud con ID {request.orderRequestId} no encontrada.");
            }

            var result = this._mapper.Map<GetOrderRequestByIdResult>(orderRequest);
            return result;
        }
    }
}
