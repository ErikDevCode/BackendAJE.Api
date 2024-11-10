namespace BackEndAje.Api.Application.Asset.Command.UpdateClientAssetReassign
{
    using MediatR;

    public class UpdateClientAssetReassignCommand : IRequest<Unit>
    {
        public int ClientAssetId { get; set; }

        public int NewClientId { get; set; }

        public string Notes { get; set; }

        public int UpdatedBy { get; set; }
    }
}

