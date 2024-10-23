namespace BackEndAje.Api.Application.Users.Queries.GetSupervisorByCedi
{
    using MediatR;

    public record GetSupervisorByCediQuery(int CediId) : IRequest<List<GetSupervisorByCediResult>>;
}

