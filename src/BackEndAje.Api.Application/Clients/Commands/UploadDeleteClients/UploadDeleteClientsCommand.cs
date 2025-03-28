namespace BackEndAje.Api.Application.Clients.Commands.UploadDeleteClients
{
    using MediatR;

    public class UploadDeleteClientsCommand : IRequest<UploadDeleteClientsResult>
    {
        public byte[] FileBytes { get; set; } = null!;

        public int UpdatedBy { get; set; }
    }
}