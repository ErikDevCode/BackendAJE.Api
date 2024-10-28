namespace BackEndAje.Api.Application.Clients.Commands.DisableClient
{
    using MediatR;

    public class DisableClientCommand : IRequest<bool>
    {
        public int ClientId { get; set; }

        public int UpdatedBy { get; set; }
    }
}
