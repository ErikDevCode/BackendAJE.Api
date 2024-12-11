namespace BackEndAje.Api.Domain.Entities
{
    public class ClientAssets : AuditableEntity
    {
        public int ClientAssetId { get; set; }
        public int CediId { get; set; }
        public DateTime? InstallationDate { get; set; }
        public int ClientId { get; set; }

        public int AssetId { get; set; }
        public string CodeAje { get; set; }
        public string Notes { get; set; }
        public bool? IsActive { get; set; }

        public Cedi Cedi { get; set; }
        public Client Client { get; set; }

        public Asset Asset { get; set; }
        public Zone Zone { get; set; }
    }
}

