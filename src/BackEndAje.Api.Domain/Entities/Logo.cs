namespace BackEndAje.Api.Domain.Entities
{
    public class Logo : AuditableEntity
    {
        public int LogoId { get; set; }
        public string LogoDescription { get; set; }
        public bool IsActive { get; set; }
    }
}
