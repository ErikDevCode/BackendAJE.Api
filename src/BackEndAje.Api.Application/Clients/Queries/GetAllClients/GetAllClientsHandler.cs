namespace BackEndAje.Api.Application.Clients.Queries.GetAllClients
{
    using AutoMapper;
    using BackEndAje.Api.Application.Abstractions.Common;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetAllClientsHandler : IRequestHandler<GetAllClientsQuery, PaginatedResult<GetAllClientsResult>>
    {
        private readonly IClientRepository _clientRepository;
        private readonly IMapper _mapper;
        private readonly ICensusRepository _censusRepository;

        public GetAllClientsHandler(IClientRepository clientRepository, IMapper mapper, ICensusRepository censusRepository)
        {
            this._clientRepository = clientRepository;
            this._mapper = mapper;
            this._censusRepository = censusRepository;
        }

        public async Task<PaginatedResult<GetAllClientsResult>> Handle(GetAllClientsQuery request, CancellationToken cancellationToken)
        {
            var clients = await this._clientRepository.GetClients(request.PageNumber, request.PageSize, request.Filtro, request.UserId);
            var result = this._mapper.Map<List<GetAllClientsResult>>(clients);

            var currentMonthPeriod = DateTime.UtcNow.ToString("yyyyMM");

            var clientIds = clients.Select(c => c.ClientId).ToList();
            var censusClientIds = await this._censusRepository
                .GetCensusClientIdsByPeriodAsync(currentMonthPeriod, clientIds);

            // Asignar IsCensus a cada cliente en el resultado
            foreach (var item in result)
            {
                var client = clients.FirstOrDefault(c => c.ClientId == item.ClientId);
                var hasAssets = client?.ClientAssets.Any() ?? false;
                var hasCensus = censusClientIds.Contains(item.ClientId);

                item.IsCensus = hasCensus;
            }

            // Obtener total de clientes para paginación
            var totalClients = await this._clientRepository.GetTotalClients(request.Filtro, request.UserId);

            // Retornar resultado paginado
            return new PaginatedResult<GetAllClientsResult>
            {
                TotalCount = totalClients,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Items = result,
            };
        }
    }
}
