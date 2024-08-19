namespace BackEndAje.Api.Application.Users.Queries.GetUsersWithRoles
{
    using MediatR;

    public record GetUsersWithRolesQuery() : IRequest<List<GetUsersWithRolesResult>>;
}