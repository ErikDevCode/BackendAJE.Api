namespace BackEndAje.Api.Application.Asset.Command.CreateAsset
{
    using BackEndAje.Api.Application.Behaviors;
    using MediatR;

    public class CreateAssetCommand : IRequest<Unit>, IHasAuditInfo
    {
        public string CodeAje { get; set; }

        public string Logo { get; set; }

        public string Brand { get; set; }

        public string Model { get; set; }

        public bool IsActive { get; set; }

        public int CreatedBy { get; set; }

        public int UpdatedBy { get; set; }
    }
}

