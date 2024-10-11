namespace BackEndAje.Api.Domain.Entities
{
    public class ProductType : AuditableEntity
    {
        public int ProductTypeId { get; set; }
        public string ProductTypeDescription { get; set; }
        public bool IsActive { get; set; }
    }
}
