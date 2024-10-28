namespace BackEndAje.Api.Application.Positions.Commands.UpdateStatusPosition
{
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class UpdateStatusPositionHandler : IRequestHandler<UpdateStatusPositionCommand, bool>
    {
        private readonly IPositionRepository _positionRepository;

        public UpdateStatusPositionHandler(IPositionRepository positionRepository)
        {
            this._positionRepository = positionRepository;
        }

        public async Task<bool> Handle(UpdateStatusPositionCommand request, CancellationToken cancellationToken)
        {
            var existingPosition = await this._positionRepository.GetPositionByIdAsync(request.PositionId);
            if (existingPosition == null)
            {
                throw new InvalidOperationException($"Cargo con ID '{request.PositionId}' no existe.");
            }

            existingPosition.IsActive = existingPosition.IsActive is false;
            await this._positionRepository.UpdatePositionAsync(existingPosition);
            return true;
        }
    }
}

