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
            var orderRequest = await this._orderRequestRepository.GetOrderRequestStatusHistoryByOrderRequestId(request.orderRequestId);

            var result = this._mapper.Map<List<GetTrackingByOrderRequestIdResult>>(orderRequest);
            return result;
        }
    }
}