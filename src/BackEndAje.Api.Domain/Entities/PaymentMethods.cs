namespace BackEndAje.Api.Domain.Entities
{
    public class PaymentMethods : AuditableEntity
    {
        public int PaymentMethodId { get; set; }
        public string PaymentMethod { get; set; }
    }
}
