namespace BackEndAje.Api.Application.Clients.Commands.UploadCodeAndNameClients
{
    using MediatR;

    public class UploadCodeAndNameClientsCommand : IRequest<UploadCodeAndNameClientsResult>
    {
        public byte[] FileBytes { get; set; } = null!;

        public int UpdatedBy { get; set; }
    }
}
