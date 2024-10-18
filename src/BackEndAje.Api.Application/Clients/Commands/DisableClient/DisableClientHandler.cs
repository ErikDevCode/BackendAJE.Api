namespace BackEndAje.Api.Application.Clients.Commands.DisableClient
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class DisableClientHandler : IRequestHandler<DisableClientCommand, Unit>
    {
        private readonly IClientRepository _clientRepository;
        private readonly IMapper _mapper;

        public DisableClientHandler(IClientRepository clientRepository, IMapper mapper)
        {
            this._clientRepository = clientRepository;
            this._mapper = mapper;
        }

        public async Task<Unit> Handle(DisableClientCommand request, CancellationToken cancellationToken)
        {
            var existingClient = await this._clientRepository.GetClientByClientCode(request.ClientCode);
            if (existingClient == null)
            {
                throw new InvalidOperationException($"Client with code '{request.ClientCode}' not exists.");
            }

            existingClient.IsActive = existingClient.IsActive is false;
            existingClient.UpdatedBy = request.UpdatedBy;
            this._mapper.Map(request, existingClient);
            await this._clientRepository.UpdateClientAsync(existingClient);
            return Unit.Value;
        }
    }
}
