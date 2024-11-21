namespace BackEndAje.Api.Application.Asset.Command.UpdateClientAssetReassign
{
    using BackEndAje.Api.Application.Behaviors;
    using MediatR;

    public class UpdateClientAssetReassignCommand : IRequest<Unit>, IHasUpdatedByInfo
    {
        public int ClientAssetId { get; set; }

        public int NewClientId { get; set; }

        public string Notes { get; set; }

        public int UpdatedBy { get; set; }
    }
}

