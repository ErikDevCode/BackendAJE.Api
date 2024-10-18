namespace BackEndAje.Api.Application.Clients.Commands.UploadClient
{
    using MediatR;

    public class UploadClientsCommand : IRequest<UploadClientsResult>
    {
        public byte[] FileBytes { get; set; }

        public int CreatedBy { get; set; }

        public int UpdatedBy { get; set; }
    }
}
