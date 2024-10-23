namespace BackEndAje.Api.Application.Clients.Commands.DisableClient
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class DisableClientHandler : IRequestHandler<DisableClientCommand, Unit>
    {
        private readonly IClientRepository _clientRepository;

        public DisableClientHandler(IClientRepository clientRepository)
        {
            this._clientRepository = clientRepository;
        }

        public async Task<Unit> Handle(DisableClientCommand request, CancellationToken cancellationToken)
        {
            var existingClient = await this._clientRepository.GetClientById(request.ClientId);
            if (existingClient == null)
            {
                throw new InvalidOperationException($"Client with code '{request.ClientId}' not exists.");
            }

            existingClient.IsActive = existingClient.IsActive is false;
            existingClient.UpdatedBy = request.UpdatedBy;
            await this._clientRepository.UpdateClientAsync(existingClient);
            return Unit.Value;
        }
    }
}
