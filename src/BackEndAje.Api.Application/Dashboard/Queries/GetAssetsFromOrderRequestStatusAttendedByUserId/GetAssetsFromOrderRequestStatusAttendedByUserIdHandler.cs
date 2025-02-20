namespace BackEndAje.Api.Application.Dashboard.Queries.GetAssetsFromOrderRequestStatusAttendedByUserId
{
    using BackEndAje.Api.Application.Dtos.Const;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetAssetsFromOrderRequestStatusAttendedByUserIdHandler : IRequestHandler<GetAssetsFromOrderRequestStatusAttendedByUserIdQuery, GetAssetsFromOrderRequestStatusAttendedByUserIdResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IOrderRequestRepository _orderRequestRepository;

        public GetAssetsFromOrderRequestStatusAttendedByUserIdHandler(IUserRepository userRepository, IOrderRequestRepository orderRequestRepository)
        {
            this._userRepository = userRepository;
            this._orderRequestRepository = orderRequestRepository;
        }

        public async Task<GetAssetsFromOrderRequestStatusAttendedByUserIdResult> Handle(GetAssetsFromOrderRequestStatusAttendedByUserIdQuery request, CancellationToken cancellationToken)
        {
            var user = await this._userRepository.GetUserByIdAsync(request.UserId);
            var role = user!.UserRoles.Select(x => x.Role.RoleName).FirstOrDefault();

            var count = await this.GetOrderRequestCountByRole(role!, request);
            var results = new GetAssetsFromOrderRequestStatusAttendedByUserIdResult()
            {
                Type = "Activos Atendidos",
                Value = count,
            };
            return results;
        }

        private async Task<int> GetOrderRequestCountByRole(string role, GetAssetsFromOrderRequestStatusAttendedByUserIdQuery requestStatus)
        {
            return role switch
            {
                RolesConst.Administrador or RolesConst.Jefe or RolesConst.ProveedorLogistico or RolesConst.Trade =>
                    await this._orderRequestRepository.GetTotalAssetFromOrderRequestStatusAttendedCount(
                        regionId: requestStatus.regionId,
                        cediId: requestStatus.cediId,
                        zoneId: requestStatus.zoneId,
                        route: requestStatus.route,
                        month: requestStatus.month,
                        year: requestStatus.year),

                RolesConst.Supervisor =>
                    await this._orderRequestRepository.GetTotalAssetFromOrderRequestStatusAttendedCount(
                        supervisorId: requestStatus.UserId,
                        month: requestStatus.month,
                        year: requestStatus.year),

                RolesConst.Vendedor =>
                    await this._orderRequestRepository.GetTotalAssetFromOrderRequestStatusAttendedCount(
                        vendedorId: requestStatus.UserId,
                        month: requestStatus.month,
                        year: requestStatus.year),

                _ => 0
            };
        }
    }
}