namespace BackEndAje.Api.Application.OrderRequests.Commands.CreateOrderRequests
{
    using BackEndAje.Api.Application.Behaviors;
    using BackEndAje.Api.Application.Dtos.OrderRequests;
    using MediatR;

    public record CreateOrderRequestsCommand : IRequest<Unit>, IHasAuditInfo
    {
        public int? SupervisorId { get; set; }

        public int CediId { get; set; }

        public int ReasonRequestId { get; set; }

        public DateTime NegotiatedDate { get; set; }

        public int TimeWindowId { get; set; }

        public int? WithDrawalReasonId { get; set; }

        public int ClientId { get; set; }

        public int? DestinationClientId { get; set; }

        public int? DestinationCediId { get; set; }

        public int ClientCode { get; set; }

        public string Observations { get; set; }

        public string Reference { get; set; }

        public int ProductSizeId { get; set; }

        public int? AssetId { get; set; }

        public List<CreateOrderRequestDocumentDto> Documents { get; set; }

        public int CreatedBy { get; set; }

        public int UpdatedBy { get; set; }
    }
}