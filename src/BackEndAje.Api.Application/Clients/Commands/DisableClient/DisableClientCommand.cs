namespace BackEndAje.Api.Application.Clients.Commands.DisableClient
{
    using BackEndAje.Api.Application.Behaviors;
    using MediatR;

    public class DisableClientCommand : IRequest<bool>, IHasUpdatedByInfo
    {
        public int ClientId { get; set; }

        public int UpdatedBy { get; set; }
    }
}
