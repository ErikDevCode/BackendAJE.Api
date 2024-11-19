namespace BackEndAje.Api.Domain.Entities
{
    public class OrderRequestAssetsTrace
    {
        public int OrderRequestAssetTraceId { get; set; }
        public int OrderRequestAssetId { get; set; }
        public int OrderRequestId { get; set; }
        public int AssetId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        
        public Asset Asset { get; set; }
        public OrderRequest OrderRequest { get; set; }
        public OrderRequestAssets OrderRequestAssets { get; set; }
        public User User { get; set; }
    }
}
