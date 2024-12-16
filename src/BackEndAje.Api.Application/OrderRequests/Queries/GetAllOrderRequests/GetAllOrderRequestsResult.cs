using BackEndAje.Api.Domain.Entities;

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

        public int CediId { get; set; }

        public string CediName { get; set; }

        public int RegionId { get; set; }

        public string RegionName { get; set; }

        public bool? IsActive { get; set; }

        public bool IsRelocation { get; set; }

        public bool IsContinue { get; set; } = true;

        public ICollection<RelocationRequest> RelocationRequest { get; set; }
    }
}