namespace BackEndAje.Api.Domain.Entities
{
    public class OrderRequestAssets: AuditableEntity
    {
        public int OrderRequestAssetId { get; set; }
        public int OrderRequestId { get; set; }
        public int AssetId { get; set; }
        public bool IsActive { get; set; }
        
        public OrderRequest OrderRequest { get; set; }
        public Asset Asset { get; set; }
    }
}