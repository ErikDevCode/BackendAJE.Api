namespace BackEndAje.Api.Application.OrderRequests.Queries.GetTrackingByOrderRequestId
{
    public class GetTrackingByOrderRequestIdResult
    {
        public int OrderRequestId { get; set; }

        public int OrderStatusId { get; set; }

        public string StatusName { get; set; }

        public string ChangeReason { get; set; }

        public DateTime CreatedAt { get; set; }

        public int CreatedBy { get; set; }

        public string ResponsibleUser { get; set; }
    }
}