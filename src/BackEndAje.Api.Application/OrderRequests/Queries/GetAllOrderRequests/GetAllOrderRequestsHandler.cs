namespace BackEndAje.Api.Application.OrderRequests.Queries.GetAllOrderRequests
{
    using AutoMapper;
    using BackEndAje.Api.Application.Abstractions.Common;
    using BackEndAje.Api.Application.Dtos.Const;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetAllOrderRequestsHandler : IRequestHandler<GetAllOrderRequestsQuery, PaginatedResult<GetAllOrderRequestsResult>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IOrderRequestRepository _orderRequestRepository;
        private readonly IMapper _mapper;

        public GetAllOrderRequestsHandler(IOrderRequestRepository orderRequestRepository, IMapper mapper, IUserRepository userRepository)
        {
            this._orderRequestRepository = orderRequestRepository;
            this._mapper = mapper;
            this._userRepository = userRepository;
        }

        public async Task<PaginatedResult<GetAllOrderRequestsResult>> Handle(GetAllOrderRequestsQuery request, CancellationToken cancellationToken)
        {
            var user = await this._userRepository.GetUserByIdAsync(request.UserId);
            var role = user!.UserRoles.Select(x => x.Role.RoleName).FirstOrDefault();

            var (supervisorId, vendedorId) = this.GetRoleFilters(role!, request.UserId);

            var orderRequests = await this._orderRequestRepository.GetAllOrderRequestAsync(
                request.PageNumber,
                request.PageSize,
                request.ClientCode,
                request.OrderStatusId,
                request.ReasonRequestId,
                request.CediId,
                request.RegionId,
                request.StartDate,
                request.EndDate,
                supervisorId,
                vendedorId);

            var totalOrderRequests = await this._orderRequestRepository.GetTotalOrderRequestCountAsync(
                request.ClientCode,
                request.OrderStatusId,
                request.ReasonRequestId,
                request.CediId,
                request.RegionId,
                request.StartDate,
                request.EndDate,
                supervisorId,
                vendedorId);

            var result = this._mapper.Map<List<GetAllOrderRequestsResult>>(orderRequests);

            foreach (var resultFirst in result)
            {
                foreach (var relocation in resultFirst.RelocationRequest)
                {
                    var listRelocationRequest = await this._orderRequestRepository.GetRelocationRequestByRelocationId(relocation.RelocationId);
                    var withdraw = listRelocationRequest.Find(x => x.ReasonRequestId == 2);
                    resultFirst.IsContinue = withdraw!.OrderStatusId == 5;
                }
            }

            var paginatedResult = new PaginatedResult<GetAllOrderRequestsResult>
            {
                TotalCount = totalOrderRequests,
                PageNumber = request.PageNumber ?? 1,
                PageSize = request.PageSize ?? totalOrderRequests,
                Items = result,
            };

            return paginatedResult;
        }

        private (int? supervisorId, int? vendedorId) GetRoleFilters(string role, int userId)
        {
            return role switch
            {
                RolesConst.Administrador or RolesConst.Jefe or RolesConst.ProveedorLogistico or RolesConst.Trade => (null, null),
                RolesConst.Supervisor => (supervisorId: userId, vendedorId: null),
                RolesConst.Vendedor => (supervisorId: null, vendedorId: userId),
                _ => (supervisorId: null, vendedorId: null),
            };
        }
    }
}

