namespace BackEndAje.Api.Application.Asset.Queries.GetClientAssetsTrace
{
    public class GetClientAssetsTraceResult
    {
        public int ClientAssetTraceId { get; set; }

        public int ClientAssetId { get; set; }

        public int? PreviousClientId { get; set; }

        public int? NewClientId { get; set; }

        public int AssetId { get; set; }

        public string CodeAje { get; set; }

        public string? ChangeReason { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public int CreatedBy { get; set; }

        // Additional fields for display purposes
        public string PreviousClientName { get; set; }

        public string NewClientName { get; set; }
    }
}

