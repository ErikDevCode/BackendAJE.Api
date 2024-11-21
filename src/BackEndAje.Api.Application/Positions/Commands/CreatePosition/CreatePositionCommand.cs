namespace BackEndAje.Api.Application.Positions.Commands.CreatePosition
{
    using BackEndAje.Api.Application.Behaviors;
    using MediatR;

    public class CreatePositionCommand : IRequest<Unit>, IHasAuditInfo
    {
        public string PositionName { get; set; }

        public int CreatedBy { get; set; }

        public int UpdatedBy { get; set; }
    }
}