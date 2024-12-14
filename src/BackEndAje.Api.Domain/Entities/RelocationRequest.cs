namespace BackEndAje.Api.Domain.Entities
{
    public class RelocationRequest : AuditableEntity
    {
        public int Id { get; set; }
        public int RelocationId { get; set; }
        
        public Relocation Relocation {get; set; }
        public int ReasonRequestId { get; set; }
        
        public ReasonRequest ReasonRequest { get; set; }
        public int OrderRequestId { get; set; }
        public int OrderStatusId { get; set; }
        
        public OrderStatus OrderStatus { get; set; }
        public bool IsActive { get; set; }
        
        
    }
}
