namespace BackEndAje.Api.Application.Clients.Commands.UpdateClient
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class UpdateClientHandler : IRequestHandler<UpdateClientCommand, Unit>
    {
        private readonly IClientRepository _clientRepository;
        private readonly IMapper _mapper;

        public UpdateClientHandler(IClientRepository clientRepository, IMapper mapper)
        {
            this._clientRepository = clientRepository;
            this._mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
        {
            var existingClient = await this._clientRepository.GetClientById(request.ClientId);
            if (existingClient == null)
            {
                throw new InvalidOperationException($"Client with ID '{request.ClientId}' not exists.");
            }

            this._mapper.Map(request, existingClient);
            await this._clientRepository.UpdateClientAsync(existingClient);
            return Unit.Value;
        }
    }
}
