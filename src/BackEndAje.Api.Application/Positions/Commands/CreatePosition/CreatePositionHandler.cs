namespace BackEndAje.Api.Application.Positions.Commands.CreatePosition
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class CreatePositionHandler : IRequestHandler<CreatePositionCommand, Unit>
    {
        private readonly IPositionRepository _positionRepository;
        private readonly IMapper _mapper;

        public CreatePositionHandler(IPositionRepository positionRepository, IMapper mapper)
        {
            this._positionRepository = positionRepository;
            this._mapper = mapper;
        }

        public async Task<Unit> Handle(CreatePositionCommand request, CancellationToken cancellationToken)
        {
            var positionExists = await this._positionRepository.PositionExistsAsync(request.PositionName);
            if (positionExists)
            {
                throw new InvalidOperationException($"Cargo '{request.PositionName}' already exists.");
            }

            var newPosition = this._mapper.Map<Position>(request);
            newPosition.IsActive = true;
            await this._positionRepository.AddPositionAsync(newPosition);
            return Unit.Value;
        }
    }
}

