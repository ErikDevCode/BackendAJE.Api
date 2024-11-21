namespace BackEndAje.Api.Application.Asset.Command.UploadAssets
{
    using BackEndAje.Api.Application.Behaviors;
    using MediatR;

    public class UploadAssetsCommand : IRequest<UploadAssetsResult>, IHasAuditInfo
    {
        public byte[] FileBytes { get; set; }

        public int CreatedBy { get; set; }

        public int UpdatedBy { get; set; }
    }
}