namespace BackEndAje.Api.Application.Asset.Command.UpdateStatusAsset
{
    using BackEndAje.Api.Application.Behaviors;
    using MediatR;

    public class UpdateStatusAssetCommand : IRequest<bool>, IHasUpdatedByInfo
    {
        public int AssetId { get; set; }

        public int UpdatedBy { get; set; }
    }
}
