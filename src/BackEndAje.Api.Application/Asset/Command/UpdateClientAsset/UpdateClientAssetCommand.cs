namespace BackEndAje.Api.Application.Asset.Command.UpdateClientAsset
{
    using BackEndAje.Api.Application.Behaviors;
    using MediatR;

    public class UpdateClientAssetCommand : IRequest<Unit>, IHasUpdatedByInfo
    {
        public int ClientAssetId { get; set; }

        public int CediId { get; set; }

        public DateTime InstallationDate { get; set; }

        public string Notes { get; set; }

        public int UpdatedBy { get; set; }
    }
}

