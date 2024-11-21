namespace BackEndAje.Api.Application.Positions.Commands.UpdatePosition
{
    using BackEndAje.Api.Application.Behaviors;
    using MediatR;

    public class UpdatePositionCommand : IRequest<Unit>, IHasUpdatedByInfo
    {
        public int PositionId { get; set; }

        public string PositionName { get; set; }

        public int UpdatedBy { get; set; }
    }
}