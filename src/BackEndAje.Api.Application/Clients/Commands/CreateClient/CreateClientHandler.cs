namespace BackEndAje.Api.Application.Clients.Commands.CreateClient
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class CreateClientHandler : IRequestHandler<CreateClientCommand, Unit>
    {
        private readonly IClientRepository _clientRepository;
        private readonly IMapper _mapper;

        public CreateClientHandler(IClientRepository clientRepository, IMapper mapper)
        {
            this._clientRepository = clientRepository;
            this._mapper = mapper;
        }

        public async Task<Unit> Handle(CreateClientCommand request, CancellationToken cancellationToken)
        {
            var existingClient = await this._clientRepository.GetClientByDocumentNumber(request.DocumentNumber);
            if (existingClient != null)
            {
                throw new InvalidOperationException($"Client '{request.DocumentNumber}' already exists.");
            }

            var newClient = this._mapper.Map<Client>(request);
            await this._clientRepository.AddClient(newClient);
            return Unit.Value;
        }
    }
}