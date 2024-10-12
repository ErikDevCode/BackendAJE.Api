namespace BackEndAje.Api.Domain.Entities
{
    public class Client : AuditableEntity
    {
        public int ClientId { get; set; }
        public int ClientCode { get; set; }
        public string CompanyName { get; set; }
        public int DocumentTypeId { get; set; }
        public DocumentType DocumentType { get; set; }
        public string DocumentNumber { get; set; }
        public string Email { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public int PaymentMethodId { get; set; }
        public PaymentMethods PaymentMethod { get; set; }
        public int? Route { get; set; }
        public User? Seller { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string DistrictId { get; set; }
        public District District { get; set; }
        public string? CoordX { get; set; }
        public string? CoordY { get; set; }
        public string? Segmentation { get; set; }
        public bool IsActive { get; set; }
    }
}
