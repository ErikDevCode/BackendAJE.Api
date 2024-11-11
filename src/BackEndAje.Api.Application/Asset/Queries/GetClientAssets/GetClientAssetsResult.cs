namespace BackEndAje.Api.Application.Asset.Queries.GetClientAssets
{
    public class GetClientAssetsResult
    {
        public int ClientAssetId { get; set; }

        public int CediId { get; set; }

        public string CediName { get; set; }

        public DateTime InstallationDate { get; set; }

        public int ClientId { get; set; }

        public int? UserId { get; set; }

        public int? Route { get; set; }

        public string? Seller { get; set; }

        public string ClientCode { get; set; }

        public string ClientName { get; set; }

        public int AssetId { get; set; }

        public string CodeAje { get; set; }

        public string Logo { get; set; }

        public string Brand { get; set; }

        public string Model { get; set; }

        public bool AssetIsActive { get; set; }

        public string Notes { get; set; }

        public bool IsActive { get; set; }
    }
}