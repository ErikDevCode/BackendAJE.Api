namespace BackEndAje.Api.Application.OrderRequests.Queries.GetTrackingAssetsByOrderRequestId
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetTrackingAssetsByOrderRequestIdHandler : IRequestHandler<GetTrackingAssetsByOrderRequestIdQuery, List<GetTrackingAssetsByOrderRequestIdResult>>
    {
        private readonly IOrderRequestRepository _orderRequestRepository;
        private readonly IMapper _mapper;

        public GetTrackingAssetsByOrderRequestIdHandler(IOrderRequestRepository orderRequestRepository, IMapper mapper)
        {
            this._orderRequestRepository = orderRequestRepository;
            this._mapper = mapper;
        }

        public async Task<List<GetTrackingAssetsByOrderRequestIdResult>> Handle(GetTrackingAssetsByOrderRequestIdQuery request, CancellationToken cancellationToken)
        {
            var orderRequestAssetTrace = await this._orderRequestRepository.GetOrderRequestAssetsTraceByOrderRequestId(request.orderRequestId);
            if (orderRequestAssetTrace == null || !orderRequestAssetTrace.Any())
            {
                throw new KeyNotFoundException($"Solicitud con ID {request.orderRequestId} no encontrado.");
            }

            var result = this._mapper.Map<List<GetTrackingAssetsByOrderRequestIdResult>>(orderRequestAssetTrace);
            return result;
        }
    }
}

