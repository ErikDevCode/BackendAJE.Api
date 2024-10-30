namespace BackEndAje.Api.Application.Asset.Command.UploadAssets
{
    using MediatR;

    public class UploadAssetsCommand : IRequest<UploadAssetsResult>
    {
        public byte[] FileBytes { get; set; }

        public int CreatedBy { get; set; }

        public int UpdatedBy { get; set; }
    }
}