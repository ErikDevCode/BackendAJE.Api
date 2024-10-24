namespace BackEndAje.Api.Application.Positions.Commands.UpdatePosition
{
    using MediatR;

    public class UpdatePositionCommand : IRequest<Unit>
    {
        public int PositionId { get; set; }

        public string PositionName { get; set; }

        public int UpdatedBy { get; set; }
    }
}