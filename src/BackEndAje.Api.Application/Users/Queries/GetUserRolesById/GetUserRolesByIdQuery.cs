namespace BackEndAje.Api.Application.Users.Queries.GetUserRolesById
{
    using MediatR;

    public record GetUserRolesByIdQuery(int UserId) : IRequest<List<string>>;
}
