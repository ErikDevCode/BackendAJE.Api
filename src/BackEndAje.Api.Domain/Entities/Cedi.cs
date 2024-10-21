namespace BackEndAje.Api.Domain.Entities
{
    public class Cedi : AuditableEntity
    {
        public int CediId { get; set; }
        
        public int RegionId { get; set; }
        
        public string CediName { get; set; }
        
        public Region? Region { get; set; }
    }
}
