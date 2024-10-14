namespace BackEndAje.Api.Domain.Entities
{
    public class OrderRequest : AuditableEntity
    {
        public int OrderRequestId { get; set; }
        public int SupervisorId { get; set; }
        public int CediId { get; set; }
        public int ReasonRequestId { get; set; }
        public DateTime NegotiatedDate { get; set; }
        public int? WithDrawalReasonId { get; set; }
        public int ClientCode { get; set; }
        public string Observations { get; set; }
        public string Reference { get; set; }
        public int ProductTypeId { get; set; }
        public int LogoId { get; set; }
        public string Modelo { get; set; }
        public int ProductSizeId { get; set; }

        public User Supervisor { get; set; }
        public Cedi Cedi { get; set; }
        public ReasonRequest ReasonRequest { get; set; }
        public TimeWindow TimeWindow { get; set; }
        public Client Client { get; set; }
        public ProductType ProductType { get; set; }
        public Logo Logo { get; set; }
        public ProductSize ProductSize { get; set; }
        public WithDrawalReason? WithDrawalReason { get; set; }  // Es opcional

        public ICollection<OrderRequestDocument> Documents { get; set; } = new List<OrderRequestDocument>();
    }
}