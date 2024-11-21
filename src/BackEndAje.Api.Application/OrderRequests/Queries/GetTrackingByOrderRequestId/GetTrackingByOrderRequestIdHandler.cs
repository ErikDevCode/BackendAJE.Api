namespace BackEndAje.Api.Application.OrderRequests.Queries.GetTrackingByOrderRequestId
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetTrackingByOrderRequestIdHandler : IRequestHandler<GetTrackingByOrderRequestIdQuery, List<GetTrackingByOrderRequestIdResult>>
    {
        private readonly IOrderRequestRepository _orderRequestRepository;
        private readonly IMapper _mapper;

        public GetTrackingByOrderRequestIdHandler(IOrderRequestRepository orderRequestRepository, IMapper mapper)
        {
            this._orderRequestRepository = orderRequestRepository;
            this._mapper = mapper;
        }

        public async Task<List<GetTrackingByOrderRequestIdResult>> Handle(GetTrackingByOrderRequestIdQuery request, CancellationToken cancellationToken)
        {
            var orderRequestHistory = await this._orderRequestRepository.GetOrderRequestStatusHistoryByOrderRequestId(request.orderRequestId);

            if (orderRequestHistory == null || !orderRequestHistory.Any())
            {
                throw new KeyNotFoundException($"No se encontró historial para la solicitud con ID {request.orderRequestId}.");
            }

            var result = this._mapper.Map<List<GetTrackingByOrderRequestIdResult>>(orderRequestHistory);
            return result;
        }
    }
}