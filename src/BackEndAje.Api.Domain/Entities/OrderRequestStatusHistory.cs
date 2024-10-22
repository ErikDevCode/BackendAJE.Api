namespace BackEndAje.Api.Domain.Entities
{
    public class OrderRequestStatusHistory
    {
        public int OrderStatusHistoryId { get; set; }
        public int OrderRequestId { get; set; }
        public int OrderStatusId { get; set; }
        public string? ChangeReason { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        
        public OrderRequest OrderRequest { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public User CreatedByUser  { get; set; }
    }
}
