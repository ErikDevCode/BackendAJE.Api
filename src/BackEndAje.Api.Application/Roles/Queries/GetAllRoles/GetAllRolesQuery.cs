namespace BackEndAje.Api.Application.Roles.Queries.GetAllRoles
{
    using MediatR;

    public record GetAllRolesQuery() : IRequest<List<GetAllRolesResult>>;
}
