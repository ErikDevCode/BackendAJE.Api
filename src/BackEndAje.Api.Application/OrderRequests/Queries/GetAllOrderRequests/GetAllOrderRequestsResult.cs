namespace BackEndAje.Api.Application.OrderRequests.Queries.GetAllOrderRequests
{
    public class GetAllOrderRequestsResult
    {
        public int OrderRequestId { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Reason { get; set; }

        public string Branch { get; set; }

        public string CompanyName { get; set; }

        public string ClientCode { get; set; }

        public string Zone { get; set; }

        public string Route { get; set; }

        public int? OrderStatusId { get; set; }

        public string StatusName { get; set; }
    }
}