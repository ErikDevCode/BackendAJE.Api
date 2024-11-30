namespace BackEndAje.Api.Application.Asset.Command.UploadClientAssets
{
    using BackEndAje.Api.Application.Behaviors;
    using MediatR;

    public class UploadClientAssetsCommand : IRequest<Unit>, IHasAuditInfo
    {
        public byte[] FileBytes { get; set; }

        public int CreatedBy { get; set; }

        public int UpdatedBy { get; set; }
    }
}
