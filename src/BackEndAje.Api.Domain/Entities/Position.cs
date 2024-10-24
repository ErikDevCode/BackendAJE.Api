namespace BackEndAje.Api.Domain.Entities
{
    public class Position : AuditableEntity
    {
        public int PositionId { get; set; }

        public string PositionName { get; set; }

        public bool IsActive { get; set; }
    }
}