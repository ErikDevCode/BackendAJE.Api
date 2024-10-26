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
            var user = await this._userRepository.GetUserByIdAsync(requestStatus.userId);
            var role = user!.UserRoles.Select(x => x.Role.RoleName).FirstOrDefault();

            var attended = 0;
            var generated = 0;
            var refused = 0;
            var approved = 0;
            var programmed = 0;
            var falseFreight = 0;
            var canceled = 0;

            switch (role)
            {
                case RolesConst.Administrador:
                    attended = await this._orderRequestRepository.GetTotalOrderRequestStatusCount(statusId: 5, regionId: requestStatus.regionId, zoneId: requestStatus.zoneId, route: requestStatus.route, month: requestStatus.month, year: requestStatus.year);
                    generated = await this._orderRequestRepository.GetTotalOrderRequestStatusCount(statusId: 1, regionId: requestStatus.regionId, zoneId: requestStatus.zoneId, route: requestStatus.route, month: requestStatus.month, year: requestStatus.year);
                    refused = await this._orderRequestRepository.GetTotalOrderRequestStatusCount(statusId: 3, regionId: requestStatus.regionId, zoneId: requestStatus.zoneId, route: requestStatus.route, month: requestStatus.month, year: requestStatus.year);
                    approved = await this._orderRequestRepository.GetTotalOrderRequestStatusCount(statusId: 2, regionId: requestStatus.regionId, zoneId: requestStatus.zoneId, route: requestStatus.route, month: requestStatus.month, year: requestStatus.year);
                    programmed = await this._orderRequestRepository.GetTotalOrderRequestStatusCount(statusId: 4, regionId: requestStatus.regionId, zoneId: requestStatus.zoneId, route: requestStatus.route, month: requestStatus.month, year: requestStatus.year);
                    falseFreight = await this._orderRequestRepository.GetTotalOrderRequestStatusCount(statusId: 6, regionId: requestStatus.regionId, zoneId: requestStatus.zoneId, route: requestStatus.route, month: requestStatus.month, year: requestStatus.year);
                    canceled = await this._orderRequestRepository.GetTotalOrderRequestStatusCount(statusId: 7, regionId: requestStatus.regionId, zoneId: requestStatus.zoneId, route: requestStatus.route, month: requestStatus.month, year: requestStatus.year);
                    break;
                case RolesConst.Jefe:
                case RolesConst.ProveedorLogistico:
                case RolesConst.Trade:
                    attended = await this._orderRequestRepository.GetTotalOrderRequestStatusCount(statusId: 5, month: requestStatus.month, year: requestStatus.year);
                    generated = await this._orderRequestRepository.GetTotalOrderRequestStatusCount(statusId: 1, month: requestStatus.month, year: requestStatus.year);
                    refused = await this._orderRequestRepository.GetTotalOrderRequestStatusCount(statusId: 3, month: requestStatus.month, year: requestStatus.year);
                    approved = await this._orderRequestRepository.GetTotalOrderRequestStatusCount(statusId: 2, month: requestStatus.month, year: requestStatus.year);
                    programmed = await this._orderRequestRepository.GetTotalOrderRequestStatusCount(statusId: 4, month: requestStatus.month, year: requestStatus.year);
                    falseFreight = await this._orderRequestRepository.GetTotalOrderRequestStatusCount(statusId: 6, month: requestStatus.month, year: requestStatus.year);
                    canceled = await this._orderRequestRepository.GetTotalOrderRequestStatusCount(statusId: 7, month: requestStatus.month, year: requestStatus.year);
                    break;
                case RolesConst.Supervisor:
                    attended = await this._orderRequestRepository.GetTotalOrderRequestStatusCount(statusId: 5, supervisorId: requestStatus.userId, month: requestStatus.month, year: requestStatus.year);
                    generated = await this._orderRequestRepository.GetTotalOrderRequestStatusCount(statusId: 1, supervisorId: requestStatus.userId, month: requestStatus.month, year: requestStatus.year);
                    refused = await this._orderRequestRepository.GetTotalOrderRequestStatusCount(statusId: 3, supervisorId: requestStatus.userId, month: requestStatus.month, year: requestStatus.year);
                    approved = await this._orderRequestRepository.GetTotalOrderRequestStatusCount(statusId: 2, supervisorId: requestStatus.userId, month: requestStatus.month, year: requestStatus.year);
                    programmed = await this._orderRequestRepository.GetTotalOrderRequestStatusCount(statusId: 4, supervisorId: requestStatus.userId, month: requestStatus.month, year: requestStatus.year);
                    falseFreight = await this._orderRequestRepository.GetTotalOrderRequestStatusCount(statusId: 6, supervisorId: requestStatus.userId, month: requestStatus.month, year: requestStatus.year);
                    canceled = await this._orderRequestRepository.GetTotalOrderRequestStatusCount(statusId: 7, supervisorId: requestStatus.userId, month: requestStatus.month, year: requestStatus.year);
                    break;
                case RolesConst.Vendedor:
                    attended = await this._orderRequestRepository.GetTotalOrderRequestStatusCount(statusId: 5, vendedorId: requestStatus.userId, month: requestStatus.month, year: requestStatus.year);
                    generated = await this._orderRequestRepository.GetTotalOrderRequestStatusCount(statusId: 1, vendedorId: requestStatus.userId, month: requestStatus.month, year: requestStatus.year);
                    refused = await this._orderRequestRepository.GetTotalOrderRequestStatusCount(statusId: 3, vendedorId: requestStatus.userId, month: requestStatus.month, year: requestStatus.year);
                    approved = await this._orderRequestRepository.GetTotalOrderRequestStatusCount(statusId: 2, vendedorId: requestStatus.userId, month: requestStatus.month, year: requestStatus.year);
                    programmed = await this._orderRequestRepository.GetTotalOrderRequestStatusCount(statusId: 4, vendedorId: requestStatus.userId, month: requestStatus.month, year: requestStatus.year);
                    falseFreight = await this._orderRequestRepository.GetTotalOrderRequestStatusCount(statusId: 6, vendedorId: requestStatus.userId, month: requestStatus.month, year: requestStatus.year);
                    canceled = await this._orderRequestRepository.GetTotalOrderRequestStatusCount(statusId: 7, vendedorId: requestStatus.userId, month: requestStatus.month, year: requestStatus.year);
                    break;
            }


            var result = new List<GetOrderRequestStatusByUserIdResult>
            {
                new GetOrderRequestStatusByUserIdResult { Type = "Atendidos", Value = attended },
                new GetOrderRequestStatusByUserIdResult { Type = "Generados", Value = generated },
                new GetOrderRequestStatusByUserIdResult { Type = "Rechazados", Value = refused },
                new GetOrderRequestStatusByUserIdResult { Type = "Aprobado", Value = approved },
                new GetOrderRequestStatusByUserIdResult { Type = "Programado", Value = programmed },
                new GetOrderRequestStatusByUserIdResult { Type = "Falso Flete", Value = falseFreight },
                new GetOrderRequestStatusByUserIdResult { Type = "Anulado", Value = canceled },
            };

            return result;
        }
    }
}
