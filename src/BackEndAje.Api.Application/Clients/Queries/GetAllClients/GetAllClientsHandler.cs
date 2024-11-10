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

        public GetAllClientsHandler(IClientRepository clientRepository, IMapper mapper)
        {
            this._clientRepository = clientRepository;
            this._mapper = mapper;
        }

        public async Task<PaginatedResult<GetAllClientsResult>> Handle(GetAllClientsQuery request, CancellationToken cancellationToken)
        {
            var clients = await this._clientRepository.GetClients(request.PageNumber, request.PageSize, request.Filtro);
            var result = this._mapper.Map<List<GetAllClientsResult>>(clients);
 
            var totalClients = await this._clientRepository.GetTotalClients(request.Filtro);
            var paginatedResult = new PaginatedResult<GetAllClientsResult>
            {
                TotalCount = totalClients,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Items = result,
            };

            return paginatedResult;
        }
    }
}
