namespace BackEndAje.Api.Application.Clients.Commands.UpdateClient
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class UpdateClientHandler : IRequestHandler<UpdateClientCommand, Unit>
    {
        private readonly IClientRepository _clientRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UpdateClientHandler(IClientRepository clientRepository, IMapper mapper, IUserRepository userRepository)
        {
            this._clientRepository = clientRepository;
            this._mapper = mapper;
            this._userRepository = userRepository;
        }

        public async Task<Unit> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
        {
            var existingClient = await this._clientRepository.GetClientById(request.ClientId);
            if (existingClient == null)
            {
                throw new InvalidOperationException($"Cliente con ID '{request.ClientId}' no existe.");
            }

            var existingUser = await this._userRepository.GetUserByRouteAsync(request.Route);
            var newClient = this._mapper.Map<Client>(request);
            newClient.CreatedAt = existingClient.CreatedAt;
            newClient.UserId = existingUser!.UserId;
            newClient.CreatedBy = existingClient.CreatedBy;
            await this._clientRepository.UpdateClientAsync(newClient);
            return Unit.Value;
        }
    }
}
