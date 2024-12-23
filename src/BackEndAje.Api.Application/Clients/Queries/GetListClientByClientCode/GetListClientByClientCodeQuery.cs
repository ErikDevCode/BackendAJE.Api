namespace BackEndAje.Api.Application.Clients.Queries.GetListClientByClientCode
{
    using MediatR;

    public record GetListClientByClientCodeQuery(int ClientCode) : IRequest<List<GetListClientByClientCodeResult>>;
}