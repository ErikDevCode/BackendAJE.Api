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
            var user = await this._userRepository.GetUserByIdAsync(request.userId);
            var role = user!.UserRoles.Select(x => x.Role.RoleName).FirstOrDefault();

            var installation = 0;
            var withdrawal = 0;
            var teamChange = 0;
            var technicalService = 0;

            switch (role)
            {
                case RolesConst.Administrador:
                    installation = await this._orderRequestRepository.GetTotalOrderRequestReasonCount(reasonRequestId: 1, regionId: request.regionId, zoneId: request.zoneId, route: request.route, month: request.month, year: request.year);
                    withdrawal = await this._orderRequestRepository.GetTotalOrderRequestReasonCount(reasonRequestId: 2, regionId: request.regionId, zoneId: request.zoneId, route: request.route, month: request.month, year: request.year);
                    teamChange = await this._orderRequestRepository.GetTotalOrderRequestReasonCount(reasonRequestId: 3, regionId: request.regionId, zoneId: request.zoneId, route: request.route, month: request.month, year: request.year);
                    technicalService = await this._orderRequestRepository.GetTotalOrderRequestReasonCount(reasonRequestId: 4, regionId: request.regionId, zoneId: request.zoneId, route: request.route, month: request.month, year: request.year);
                    break;
                case RolesConst.Jefe:
                case RolesConst.ProveedorLogistico:
                case RolesConst.Trade:
                    installation = await this._orderRequestRepository.GetTotalOrderRequestReasonCount(reasonRequestId: 1, month: request.month, year: request.year);
                    withdrawal = await this._orderRequestRepository.GetTotalOrderRequestReasonCount(reasonRequestId: 2, month: request.month, year: request.year);
                    teamChange = await this._orderRequestRepository.GetTotalOrderRequestReasonCount(reasonRequestId: 3, month: request.month, year: request.year);
                    technicalService = await this._orderRequestRepository.GetTotalOrderRequestReasonCount(reasonRequestId: 4, month: request.month, year: request.year);
                    break;
                case RolesConst.Supervisor:
                    installation = await this._orderRequestRepository.GetTotalOrderRequestReasonCount(reasonRequestId: 1, supervisorId: request.userId, month: request.month, year: request.year);
                    withdrawal = await this._orderRequestRepository.GetTotalOrderRequestReasonCount(reasonRequestId: 2, supervisorId: request.userId, month: request.month, year: request.year);
                    teamChange = await this._orderRequestRepository.GetTotalOrderRequestReasonCount(reasonRequestId: 3, supervisorId: request.userId, month: request.month, year: request.year);
                    technicalService = await this._orderRequestRepository.GetTotalOrderRequestReasonCount(reasonRequestId: 4, supervisorId: request.userId, month: request.month, year: request.year);
                    break;
                case RolesConst.Vendedor:
                    installation = await this._orderRequestRepository.GetTotalOrderRequestReasonCount(reasonRequestId: 1, vendedorId: request.userId, month: request.month, year: request.year);
                    withdrawal = await this._orderRequestRepository.GetTotalOrderRequestReasonCount(reasonRequestId: 2, vendedorId: request.userId, month: request.month, year: request.year);
                    teamChange = await this._orderRequestRepository.GetTotalOrderRequestReasonCount(reasonRequestId: 3, vendedorId: request.userId, month: request.month, year: request.year);
                    technicalService = await this._orderRequestRepository.GetTotalOrderRequestReasonCount(reasonRequestId: 4, vendedorId: request.userId, month: request.month, year: request.year);
                    break;
            }


            var result = new List<GetOrderRequestReasonByUserIdResult>
            {
                new GetOrderRequestReasonByUserIdResult { Type = "Instalación", Value = installation },
                new GetOrderRequestReasonByUserIdResult { Type = "Retiro", Value = withdrawal },
                new GetOrderRequestReasonByUserIdResult { Type = "Cambio de Equipo", Value = teamChange },
                new GetOrderRequestReasonByUserIdResult { Type = "Servicio Técnico", Value = technicalService },
            };

            return result;
        }
    }
}