namespace BackEndAje.Api.Application.Positions.Queries.GetAllPositions
{
    public class GetAllPositionsResult
    {
        public int PositionId { get; set; }

        public string PositionName { get; set; }

        public bool IsActive { get; set; }
    }
}