namespace BackEndAje.Api.Application.OrderRequests.Commands.CreateOrderRequests
{
    public class CreateOrderRequestsCommand
    {
        public int SupervisorId { get; set; }

        public int CediId { get; set; }

        public int ReasonRequestId { get; set; }

        public DateTime NegotiatedDate { get; set; }

        public int? WithDrawalReasonId { get; set; }

        public int ClientCode { get; set; }

        public string Observations { get; set; }

        private string Reference { get; set; }

        public int ProductTypeId { get; set; }

        public int LogoId { get; set; }

        public string Modelo { get; set; }

        public int ProductSizeId { get; set; }

        public int CreatedBy { get; set; }

        public int UpdatedBy { get; set; }
    }
}