namespace BackEndAje.Api.Application.Clients.Commands.DisableClient
{
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class DisableClientHandler : IRequestHandler<DisableClientCommand, bool>
    {
        private readonly IClientRepository _clientRepository;

        public DisableClientHandler(IClientRepository clientRepository)
        {
            this._clientRepository = clientRepository;
        }

        public async Task<bool> Handle(DisableClientCommand request, CancellationToken cancellationToken)
        {
            var existingClient = await this._clientRepository.GetClientById(request.ClientId);
            if (existingClient == null)
            {
                throw new InvalidOperationException($"Cliente con ID '{request.ClientId}' no existe.");
            }

            existingClient.IsActive = existingClient.IsActive is false;
            existingClient.UpdatedBy = request.UpdatedBy;
            await this._clientRepository.UpdateClientAsync(existingClient);
            return true;
        }
    }
}
