namespace BackEndAje.Api.Domain.Entities
{
    public class Relocation : AuditableEntity
    {
        public int RelocationId { get; set; }
        public int OriginClientId { get; set; }
        public int DestinationClientId { get; set; }
        public int TransferredAssetId { get; set; }
        public bool IsActive { get; set; }
    }
}