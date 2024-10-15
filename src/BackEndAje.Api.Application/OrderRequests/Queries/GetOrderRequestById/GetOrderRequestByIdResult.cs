namespace BackEndAje.Api.Application.OrderRequests.Queries.GetOrderRequestById
{
    using BackEndAje.Api.Application.Dtos.Cedi;
    using BackEndAje.Api.Application.Dtos.Client;
    using BackEndAje.Api.Application.Dtos.OrderRequests;
    using BackEndAje.Api.Application.Dtos.Product;
    using BackEndAje.Api.Application.Dtos.ReasonRequest;
    using BackEndAje.Api.Application.Dtos.TimeWindows;
    using BackEndAje.Api.Application.Dtos.Users;
    using BackEndAje.Api.Application.Dtos.WithDrawalReason;

    public class GetOrderRequestByIdResult
    {
        public int OrderRequestId { get; set; }

        public int SupervisorId { get; set; }

        public SupervisorDto Supervisor { get; set; }

        public int CediId { get; set; }

        public CediDto Sucursal { get; set; }

        public int ReasonRequestId { get; set; }

        public ReasonRequestDto ReasonRequest { get; set; }

        public DateTime NegotiatedDate { get; set; }

        public int TimeWindowId { get; set; }

        public TimeWindowDto TimeWindow { get; set; }

        public int? WithDrawalReasonId { get; set; }

        public WithDrawalReasonDto WithDrawalReason { get; set; }

        public int ClientId { get; set; }

        public int ClientCode { get; set; }

        public ClientDto Client { get; set; }

        public string Observations { get; set; }

        public string Reference { get; set; }

        public int ProductTypeId { get; set; }

        public ProductTypeDto ProductType { get; set; }

        public int LogoId { get; set; }

        public LogoDto Logo { get; set; }

        public string Modelo { get; set; }

        public int ProductSizeId { get; set; }

        public ProductSizeDto ProductSize { get; set; }

        public List<OrderRequestDocumentDto> OrderRequestDocuments { get; set; }
    }
}
