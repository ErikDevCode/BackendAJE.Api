namespace BackEndAje.Api.Application.Asset.Command.DeleteClientAsset
{
    using MediatR;

    public class DeleteClientAssetCommand : IRequest<Unit>
    {
        public int ClientAssetId { get; set; }
    }
}