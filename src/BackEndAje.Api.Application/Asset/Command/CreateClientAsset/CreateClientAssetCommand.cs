namespace BackEndAje.Api.Application.Asset.Command.CreateClientAsset
{
    using MediatR;

    public class CreateClientAssetCommand : IRequest<Unit>
    {
        public int CediId { get; set; }

        public DateTime InstallationDate { get; set; }

        public int ClientId { get; set; }

        public string CodeAje { get; set; }

        public string Notes { get; set; }

        public bool IsActive { get; set; }

        public bool IsCurrent { get; set; }

        public int CreatedBy { get; set; }

        public int UpdatedBy { get; set; }
    }
}