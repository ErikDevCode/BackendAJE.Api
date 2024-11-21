namespace BackEndAje.Api.Application.OrderRequests.Commands.BulkInsertOrderRequests
{
    using BackEndAje.Api.Application.Behaviors;
    using MediatR;
    using Microsoft.AspNetCore.Http;

    public class BulkInsertOrderRequestsCommand : IRequest<Unit>, IHasAuditInfo
    {
        public byte[] File { get; set; }

        public int CreatedBy { get; set; }

        public int UpdatedBy { get; set; }
    }
}