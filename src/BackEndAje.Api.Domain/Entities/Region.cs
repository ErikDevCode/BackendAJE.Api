namespace BackEndAje.Api.Domain.Entities
{
    public class Region : AuditableEntity
    {
        public int RegionId { get; set; }
        
        
        public string RegionName { get; set; }
    }
}
