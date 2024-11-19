namespace BackEndAje.Api.Application.OrderRequests.Queries.GetTrackingAssetsByOrderRequestId
{
    public class GetTrackingAssetsByOrderRequestIdResult
    {
        public int OrderRequestId { get; set; }

        public int AssetId { get; set; }

        public string CodeAje { get; set; }

        public string AssetName { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public int CreatedBy { get; set; }

        public string ResponsibleUser { get; set; }
    }
}
