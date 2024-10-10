namespace BackEndAje.Api.Domain.Entities
{
    public class WithDrawalReason : AuditableEntity
    {
        public int WithDrawalReasonId { get; set; }
        public int ReasonRequestId { get; set; }
        public string WithDrawalReasonDescription { get; set; }
        public bool IsActive { get; set; }
    }
}