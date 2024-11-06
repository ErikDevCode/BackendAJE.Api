namespace BackEndAje.Api.Domain.Entities
{
    public class ClientAssetsDto
    {
        public int ClientAssetId { get; set; }
        
        public int AssetId { get; set; }
        public string CodeAje { get; set; }
        
        public string Logo { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public bool AssetIsActive { get; set; }
        public DateTime InstallationDate { get; set; }
        public int CediId { get; set; }
        public string? CediName { get; set; }
        public int ClientId { get; set; }
        public int ClientCode { get; set; }
        public string ClientName { get; set; }
        public string? Notes { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

