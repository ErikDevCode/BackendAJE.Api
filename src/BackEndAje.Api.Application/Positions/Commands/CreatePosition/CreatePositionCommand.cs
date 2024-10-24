namespace BackEndAje.Api.Application.Positions.Commands.CreatePosition
{
    using MediatR;

    public class CreatePositionCommand : IRequest<Unit>
    {
        public string PositionName { get; set; }

        public int CreatedBy { get; set; }

        public int UpdatedBy { get; set; }
    }
}