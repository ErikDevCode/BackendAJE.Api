namespace BackEndAje.Api.Application.Clients.Queries.GetListClientByClientCode
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetListClientByClientCodeHandler  : IRequestHandler<GetListClientByClientCodeQuery, List<GetListClientByClientCodeResult>>
    {
        private readonly IClientRepository _clientRepository;
        private readonly IMapper _mapper;

        public GetListClientByClientCodeHandler(IClientRepository clientRepository, IMapper mapper)
        {
            this._clientRepository = clientRepository;
            this._mapper = mapper;
        }

        public async Task<List<GetListClientByClientCodeResult>> Handle(GetListClientByClientCodeQuery request, CancellationToken cancellationToken)
        {
            var clients = await this._clientRepository.GetListClientByClientCode(request.ClientCode);
            var result = this._mapper.Map<List<GetListClientByClientCodeResult>>(clients);
            return result;
        }
    }
}