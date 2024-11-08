namespace BackEndAje.Api.Domain.Entities
{
    public class OrderRequestDocument : AuditableEntity
    {
        public int DocumentId { get; set; }
        public int OrderRequestId { get; set; }
        public string DocumentName { get; set; }
        public decimal DocumentWeight { get; set; }
        
        public string ContentType { get; set; }
        
        public string FileFolder { get; set; }
        public string Url { get; set; }
        public bool IsActive { get; set; }
        
        public OrderRequest OrderRequest { get; set; }
    }
}
