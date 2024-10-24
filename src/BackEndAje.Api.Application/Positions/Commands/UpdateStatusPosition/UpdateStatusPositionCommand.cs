namespace BackEndAje.Api.Application.Positions.Commands.UpdateStatusPosition
{
    using MediatR;

    public class UpdateStatusPositionCommand : IRequest<bool>
    {
        public int PositionId { get; set; }

        public int UpdatedBy { get; set; }
    }
}

