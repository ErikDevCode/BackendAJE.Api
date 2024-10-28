namespace BackEndAje.Api.Application.Asset.Command.UpdateAsset
{
    using MediatR;

    public class UpdateAssetCommand : IRequest<Unit>
    {
        public int AssetId { get; set; }

        public string CodeAje { get; set; }

        public string Logo { get; set; }

        public string? AssetType { get; set; }

        public string Brand { get; set; }

        public string Model { get; set; }

        public bool IsActive { get; set; }

        public int UpdatedBy { get; set; }
    }
}