namespace BackEndAje.Api.Application.Positions.Commands.UpdatePosition
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class UpdatePositionHandler : IRequestHandler<UpdatePositionCommand, Unit>
    {
        private readonly IPositionRepository _positionRepository;
        private readonly IMapper _mapper;

        public UpdatePositionHandler(IPositionRepository positionRepository, IMapper mapper)
        {
            this._positionRepository = positionRepository;
            this._mapper = mapper;
        }

        public async Task<Unit> Handle(UpdatePositionCommand request, CancellationToken cancellationToken)
        {
            var existingPosition = await this._positionRepository.GetPositionByIdAsync(request.PositionId);
            if (existingPosition == null)
            {
                throw new InvalidOperationException($"Cargo '{request.PositionName}' not exists.");
            }

            this._mapper.Map(request, existingPosition);
            await this._positionRepository.UpdatePositionAsync(existingPosition);
            return Unit.Value;
        }
    }
}
