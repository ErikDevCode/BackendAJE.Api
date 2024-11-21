namespace BackEndAje.Api.Application.Dashboard.Queries.GetOrderRequestReasonByUserId
{
    using BackEndAje.Api.Application.Dtos.Const;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetOrderRequestReasonByUserIdHandler : IRequestHandler<GetOrderRequestReasonByUserIdQuery, List<GetOrderRequestReasonByUserIdResult>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IOrderRequestRepository _orderRequestRepository;

        public GetOrderRequestReasonByUserIdHandler(IUserRepository userRepository, IOrderRequestRepository orderRequestRepository)
        {
            this._userRepository = userRepository;
            this._orderRequestRepository = orderRequestRepository;
        }

        public async Task<List<GetOrderRequestReasonByUserIdResult>> Handle(GetOrderRequestReasonByUserIdQuery request, CancellationToken cancellationToken)
        {
            var user = await this._userRepository.GetUserByIdAsync(request.UserId);
            var role = user!.UserRoles.Select(x => x.Role.RoleName).FirstOrDefault();

            var (supervisorId, vendedorId) = this.GetRoleFilters(role!, request.UserId);

            var reasons = new Dictionary<string, int>
            {
                { "Instalación", 1 },
                { "Retiro", 2 },
                { "Cambio de Equipo", 3 },
                { "Servicio Técnico", 4 },
            };

            var result = new List<GetOrderRequestReasonByUserIdResult>();

            foreach (var reason in reasons)
            {
                var count = await this._orderRequestRepository.GetTotalOrderRequestReasonCount(
                    reasonRequestId: reason.Value,
                    supervisorId: supervisorId,
                    vendedorId: vendedorId,
                    regionId: request.regionId,
                    zoneId: request.zoneId,
                    route: request.route,
                    month: request.month,
                    year: request.year);

                result.Add(new GetOrderRequestReasonByUserIdResult { Type = reason.Key, Value = count });
            }

            return result;
        }

        private (int? supervisorId, int? vendedorId) GetRoleFilters(string role, int userId)
        {
            return role switch
            {
                RolesConst.Administrador or RolesConst.Jefe or RolesConst.ProveedorLogistico or RolesConst.Trade => (null, null),
                RolesConst.Supervisor => (userId, null),
                RolesConst.Vendedor => (null, userId),
                _ => (null, null)
            };
        }
    }
}