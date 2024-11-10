namespace BackEndAje.Api.Domain.Entities
{
    public class ClientAssetsTrace
    {
        public int ClientAssetTraceId { get; set; }
        public int ClientAssetId { get; set; }
        public int? PreviousClientId { get; set; }
        public int NewClientId { get; set; }
        public int AssetId { get; set; }
        public string CodeAje { get; set; }
        public string? ChangeReason { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int CreatedBy { get; set; }
    }
}

