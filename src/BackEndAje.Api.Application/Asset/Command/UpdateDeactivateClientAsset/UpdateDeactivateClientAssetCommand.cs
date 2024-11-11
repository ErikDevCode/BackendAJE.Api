namespace BackEndAje.Api.Application.Asset.Command.UpdateDeactivateClientAsset
{
    using MediatR;

    public class UpdateDeactivateClientAssetCommand : IRequest<bool>
    {
        public int ClientAssetId { get; set; }

        public string Notes { get; set; }

        public int UpdatedBy { get; set; }
    }
}

