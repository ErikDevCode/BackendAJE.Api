namespace BackEndAje.Api.Domain.Entities
{
    public class ReasonRequest : AuditableEntity
    {
        public int ReasonRequestId { get; set; }
        public string ReasonDescription { get; set; }
        public bool IsActive { get; set; }
    }
}
