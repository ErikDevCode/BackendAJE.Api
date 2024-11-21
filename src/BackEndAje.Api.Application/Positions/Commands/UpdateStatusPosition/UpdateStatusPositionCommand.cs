namespace BackEndAje.Api.Application.Positions.Commands.UpdateStatusPosition
{
    using BackEndAje.Api.Application.Behaviors;
    using MediatR;

    public class UpdateStatusPositionCommand : IRequest<bool>, IHasUpdatedByInfo
    {
        public int PositionId { get; set; }

        public int UpdatedBy { get; set; }
    }
}

