namespace BackEndAje.Api.Application.Clients.Queries.GetClientByClientCode
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetClientByClientCodeHandler : IRequestHandler<GetClientByClientCodeQuery, GetClientByClientCodeResult>
    {
        private readonly IClientRepository _clientRepository;
        private readonly IMastersRepository _mastersRepository;
        private readonly IMapper _mapper;

        public GetClientByClientCodeHandler(IClientRepository clientRepository, IMapper mapper, IMastersRepository mastersRepository)
        {
            this._clientRepository = clientRepository;
            this._mapper = mapper;
            this._mastersRepository = mastersRepository;
        }

        public async Task<GetClientByClientCodeResult> Handle(GetClientByClientCodeQuery request, CancellationToken cancellationToken)
        {
            // Obtener las razones de solicitud
            var reasonRequest = await this._mastersRepository.GetAllReasonRequest();

            var reasonMap = new Dictionary<string, int>();
            foreach (var reason in reasonRequest)
            {
                reasonMap[reason.ReasonDescription] = reason.ReasonRequestId;
            }

            if (!reasonMap.TryGetValue("Instalación", out var reasonInstallation) ||
                !reasonMap.TryGetValue("Retiro", out var reasonWithdrawal) ||
                !reasonMap.TryGetValue("Cambio de Equipo", out var reasonTeamChange) ||
                !reasonMap.TryGetValue("Servicio Técnico", out var reasonTechnicalService) ||
                !reasonMap.TryGetValue("Reubicación", out var reasonRelocation))
            {
                throw new InvalidOperationException("No se encontraron todas las razones requeridas en la base de datos.");
            }

            object? clients = null;

            // Selección de lógica según el tipo de solicitud
            switch (request.reasonRequestId)
            {
                case var reason when reason == reasonInstallation:
                    clients = await this._clientRepository.GetClientByClientCodeAndRoute(request.ClientCode, request.CediId, request.route);
                    break;

                case var reason when reason == reasonWithdrawal:
                    clients = await this._clientRepository.GetClientByClientCodeAndRouteWithAsset(request.ClientCode, request.CediId, request.route);
                    break;

                case var reason when reason == reasonTeamChange:
                    clients = await this._clientRepository.GetClientByClientCodeAndRouteWithAsset(request.ClientCode, request.CediId, request.route);
                    break;

                case var reason when reason == reasonTechnicalService:
                    clients = await this._clientRepository.GetClientByClientCodeAndRouteWithAsset(request.ClientCode, request.CediId, request.route);
                    break;

                case var reason when reason == reasonRelocation:
                    clients = await this._clientRepository.GetClientByClientCodeAndRouteWithAsset(request.ClientCode, request.CediId, request.route);
                    break;

                default:
                    clients = await this._clientRepository.GetClientByClientCodeAndRoute(request.ClientCode, request.CediId, request.route);
                    break;
            }

            if (clients == null)
            {
                throw new KeyNotFoundException($"Cliente con código {request.ClientCode} no encontrado en la sucursal.");
            }

            // Mapear el resultado al DTO
            var result = this._mapper.Map<GetClientByClientCodeResult>(clients);
            return result;
        }
    }
}
