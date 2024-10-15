namespace BackEndAje.Api.Application.OrderRequests.Commands.CreateOrderRequests
{
    using BackEndAje.Api.Application.Dtos.OrderRequests;
    using MediatR;

    public class CreateOrderRequestsCommand : IRequest<Unit>
    {
        public int SupervisorId { get; set; }

        public int CediId { get; set; }

        public int ReasonRequestId { get; set; }

        public DateTime NegotiatedDate { get; set; }

        public int TimeWindowId { get; set; }

        public int? WithDrawalReasonId { get; set; }

        public int ClientId { get; set; }

        public int ClientCode { get; set; }

        public string Observations { get; set; }

        public string Reference { get; set; }

        public int ProductTypeId { get; set; }

        public int LogoId { get; set; }

        public string Modelo { get; set; }

        public int ProductSizeId { get; set; }

        public List<CreateOrderRequestDocumentDto> Documents { get; set; }

        public int CreatedBy { get; set; }

        public int UpdatedBy { get; set; }
    }
}