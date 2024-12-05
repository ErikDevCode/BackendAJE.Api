namespace BackEndAje.Api.Application.Dtos.OrderRequests
{
    using BackEndAje.Api.Application.Dtos.Asset;

    public class OrderRequestAssetDto
    {
        public int OrderRequestAssetId { get; set; }

        public int OrderRequestId { get; set; }

        public int AssetId { get; set; }

        public bool IsActive { get; set; }

        public AssetDto AssetDto { get; set; }
    }
}

