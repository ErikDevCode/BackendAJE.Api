namespace BackEndAje.Api.Domain.Entities
{
    public abstract class AuditableEntity
    {
        public DateTime CreatedAt { get; set; }
        
        public DateTime UpdatedAt { get; set; }
        
        public int? CreatedBy { get; set; }
        
        public int? UpdatedBy { get; set; }
    }
}
