namespace BackEndAje.Api.Application.Asset.Command.UpdateStatusAsset
{
    using MediatR;

    public class UpdateStatusAssetCommand : IRequest<bool>
    {
        public int AssetId { get; set; }

        public int UpdatedBy { get; set; }
    }
}
