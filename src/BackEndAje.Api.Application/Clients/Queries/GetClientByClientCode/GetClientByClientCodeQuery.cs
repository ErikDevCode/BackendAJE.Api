namespace BackEndAje.Api.Application.Clients.Queries.GetClientByClientCode
{
    using MediatR;

    public record GetClientByClientCodeQuery(int ClientCode, int CediId, int route) : IRequest<GetClientByClientCodeResult>;
}