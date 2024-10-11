namespace BackEndAje.Api.Domain.Entities
{
    public class ProductSize : AuditableEntity
    {
        public int ProductSizeId { get; set; }
        public string ProductSizeDescription { get; set; }
        public bool IsActive { get; set; }
    }
}
