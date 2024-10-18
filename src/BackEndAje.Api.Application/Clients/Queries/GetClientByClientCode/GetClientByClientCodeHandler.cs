namespace BackEndAje.Api.Application.Clients.Queries.GetClientByClientCode
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetClientByClientCodeHandler : IRequestHandler<GetClientByClientCodeQuery, GetClientByClientCodeResult>
    {
        private readonly IClientRepository _clientRepository;
        private readonly IMapper _mapper;

        public GetClientByClientCodeHandler(IClientRepository clientRepository, IMapper mapper)
        {
            this._clientRepository = clientRepository;
            this._mapper = mapper;
        }

        public async Task<GetClientByClientCodeResult> Handle(GetClientByClientCodeQuery request, CancellationToken cancellationToken)
        {
            var clients = await this._clientRepository.GetClientByClientCode(request.ClientCode);
            var result = this._mapper.Map<GetClientByClientCodeResult>(clients);
            return result;
        }
    }
}
