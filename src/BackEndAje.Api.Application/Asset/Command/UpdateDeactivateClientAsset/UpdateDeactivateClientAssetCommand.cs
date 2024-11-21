namespace BackEndAje.Api.Application.Asset.Command.UpdateDeactivateClientAsset
{
    using BackEndAje.Api.Application.Behaviors;
    using MediatR;

    public class UpdateDeactivateClientAssetCommand : IRequest<bool>, IHasUpdatedByInfo
    {
        public int ClientAssetId { get; set; }

        public string Notes { get; set; }

        public int UpdatedBy { get; set; }
    }
}

