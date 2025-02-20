namespace BackEndAje.Api.Application.Dashboard.Queries.GetOrderRequestStatusByUserId
{
    using BackEndAje.Api.Application.Dtos.Const;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetOrderRequestStatusByUserIdHandler : IRequestHandler<GetOrderRequestStatusByUserIdQuery, List<GetOrderRequestStatusByUserIdResult>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IOrderRequestRepository _orderRequestRepository;

        public GetOrderRequestStatusByUserIdHandler(IOrderRequestRepository orderRequestRepository, IUserRepository userRepository)
        {
            this._orderRequestRepository = orderRequestRepository;
            this._userRepository = userRepository;
        }

        public async Task<List<GetOrderRequestStatusByUserIdResult>> Handle(GetOrderRequestStatusByUserIdQuery requestStatus, CancellationToken cancellationToken)
        {
            var user = await this._userRepository.GetUserByIdAsync(requestStatus.UserId);
            var role = user!.UserRoles.Select(x => x.Role.RoleName).FirstOrDefault();

            var statusIds = new Dictionary<string, int>
            {
                { "Atendido", 5 },
                { "Generado", 1 },
                { "Rechazado", 3 },
                { "Aprobado", 2 },
                { "Programado", 4 },
                { "Falso Flete", 6 },
                { "Anulado", 7 },
            };

            var results = new List<GetOrderRequestStatusByUserIdResult>();

            foreach (var (statusName, statusId) in statusIds)
            {
                var count = await this.GetOrderRequestCountByRole(role!, statusId, requestStatus);
                results.Add(new GetOrderRequestStatusByUserIdResult { Type = statusName, Value = count });
            }

            return results;
        }

        private async Task<int> GetOrderRequestCountByRole(string role, int statusId, GetOrderRequestStatusByUserIdQuery requestStatus)
        {
            return role switch
            {
                RolesConst.Administrador or RolesConst.Jefe or RolesConst.ProveedorLogistico or RolesConst.Trade =>
                    await this._orderRequestRepository.GetTotalOrderRequestStatusCount(
                        statusId: statusId,
                        regionId: requestStatus.regionId,
                        cediId: requestStatus.cediId,
                        zoneId: requestStatus.zoneId,
                        route: requestStatus.route,
                        month: requestStatus.month,
                        year: requestStatus.year),

                RolesConst.Supervisor =>
                    await this._orderRequestRepository.GetTotalOrderRequestStatusCount(
                        statusId: statusId,
                        supervisorId: requestStatus.UserId,
                        month: requestStatus.month,
                        year: requestStatus.year),

                RolesConst.Vendedor =>
                    await this._orderRequestRepository.GetTotalOrderRequestStatusCount(
                        statusId: statusId,
                        vendedorId: requestStatus.UserId,
                        month: requestStatus.month,
                        year: requestStatus.year),

                _ => 0
            };
        }
    }
}
