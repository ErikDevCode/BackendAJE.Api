namespace BackEndAje.Api.Application.Asset.Queries.GetAllAssets
{
    public class GetAllAssetsResult
    {
        public int AssetId { get; set; }

        public string CodeAje { get; set; }

        public string Logo { get; set; }

        public string? AssetType { get; set; }

        public string Brand { get; set; }

        public string Model { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public int CreatedBy { get; set; }

        public int UpdatedBy { get; set; }
    }
}

