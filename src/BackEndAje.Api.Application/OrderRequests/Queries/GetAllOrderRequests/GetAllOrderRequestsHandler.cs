namespace BackEndAje.Api.Application.OrderRequests.Queries.GetAllOrderRequests
{
    using AutoMapper;
    using BackEndAje.Api.Application.Abstractions.Common;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetAllOrderRequestsHandler : IRequestHandler<GetAllOrderRequestsQuery, PaginatedResult<GetAllOrderRequestsResult>>
    {
        private readonly IOrderRequestRepository _orderRequestRepository;
        private readonly IMapper _mapper;

        public GetAllOrderRequestsHandler(IOrderRequestRepository orderRequestRepository, IMapper mapper)
        {
            this._orderRequestRepository = orderRequestRepository;
            this._mapper = mapper;
        }

        public async Task<PaginatedResult<GetAllOrderRequestsResult>> Handle(GetAllOrderRequestsQuery request, CancellationToken cancellationToken)
        {
            var orderRequests = await this._orderRequestRepository.GetAllOrderRequestAsync(request.PageNumber, request.PageSize);
            var totalOrderRequests = await this._orderRequestRepository.GetTotalOrderRequestCountAsync();
            var result = this._mapper.Map<List<GetAllOrderRequestsResult>>(orderRequests);
            var paginatedResult = new PaginatedResult<GetAllOrderRequestsResult>
            {
                TotalCount = totalOrderRequests,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Items = result,
            };

            return paginatedResult;
        }
    }
}

