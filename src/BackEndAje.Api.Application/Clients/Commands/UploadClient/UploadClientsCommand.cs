namespace BackEndAje.Api.Application.Clients.Commands.UploadClient
{
    using BackEndAje.Api.Application.Behaviors;
    using MediatR;

    public class UploadClientsCommand : IRequest<UploadClientsResult>, IHasAuditInfo
    {
        public byte[] FileBytes { get; set; }

        public int CreatedBy { get; set; }

        public int UpdatedBy { get; set; }
    }
}
