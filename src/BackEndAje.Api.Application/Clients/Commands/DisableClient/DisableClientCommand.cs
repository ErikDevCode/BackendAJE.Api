namespace BackEndAje.Api.Application.Clients.Commands.DisableClient
{
    using MediatR;

    public class DisableClientCommand : IRequest<Unit>
    {
        public int ClientCode { get; set; }

        public int UpdatedBy { get; set; }
    }
}
