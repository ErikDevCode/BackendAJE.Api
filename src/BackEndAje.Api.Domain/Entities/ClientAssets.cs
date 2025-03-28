namespace BackEndAje.Api.Domain.Entities
{
    using System.Text.Json.Serialization;
    public class ClientAssets : AuditableEntity
    {
        public int ClientAssetId { get; set; }
        public int? CediId { get; set; }
        public DateTime? InstallationDate { get; set; }
        public int ClientId { get; set; }

        public int AssetId { get; set; }
        public string CodeAje { get; set; }
        public string Notes { get; set; }
        public bool? IsActive { get; set; }

        public Cedi Cedi { get; set; }
        
        [JsonIgnore]
        public Client Client { get; set; }

        public Asset Asset { get; set; }
        
        public List<ClientAssetsTrace> Traces { get; set; } = new List<ClientAssetsTrace>();
    }
}

