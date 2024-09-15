namespace BackEndAje.Api.Domain.Entities
{
    public class Zone : AuditableEntity
    {
        public int ZoneId { get; set; }
        public int CediId { get; set; }
        public int ZoneCode { get; set; }
        public string ZoneName { get; set; }
    }
}
