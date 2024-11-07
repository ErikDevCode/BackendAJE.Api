namespace BackEndAje.Api.Application.Roles.Queries.GetAllRolesWithPermissions
{
    using MediatR;

    public record GetAllRolesWithPermissionsQuery(int? roleId) : IRequest<List<GetAllRolesWithPermissionsResult>>;
}
