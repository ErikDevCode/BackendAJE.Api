namespace BackEndAje.Api.Domain.Entities
{
    public class TimeWindow : AuditableEntity
    {
        public int TimeWindowId { get; set; }
        
        public string TimeRange { get; set; }
        
        public bool IsActive { get; set; }
    }
}
