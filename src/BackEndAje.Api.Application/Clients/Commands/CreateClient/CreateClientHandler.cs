namespace BackEndAje.Api.Application.Clients.Commands.CreateClient
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class CreateClientHandler : IRequestHandler<CreateClientCommand, Unit>
    {
        private readonly IClientRepository _clientRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public CreateClientHandler(IClientRepository clientRepository, IMapper mapper, IUserRepository userRepository)
        {
            this._clientRepository = clientRepository;
            this._mapper = mapper;
            this._userRepository = userRepository;
        }

        public async Task<Unit> Handle(CreateClientCommand request, CancellationToken cancellationToken)
        {
            var existingClient = await this._clientRepository.GetClientByDocumentNumber(request.DocumentNumber);
            if (existingClient != null)
            {
                throw new InvalidOperationException($"Cliente con documento: '{request.DocumentNumber}' ya existe.");
            }

            var existingUser = await this._userRepository.GetUserByRouteAsync(request.Route);
            var newClient = this._mapper.Map<Client>(request);
            newClient.UserId = existingUser!.UserId;
            await this._clientRepository.AddClient(newClient);
            return Unit.Value;
        }
    }
}