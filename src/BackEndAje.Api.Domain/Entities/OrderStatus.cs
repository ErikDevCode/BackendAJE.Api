namespace BackEndAje.Api.Domain.Entities
{
    public class OrderStatus : AuditableEntity
    {
        public int OrderStatusId { get; set; }
        
        public string StatusName { get; set; }
        
        public bool IsActive { get; set; }
    }
}
