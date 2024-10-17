namespace BackEndAje.Api.Application.Roles.Queries.GetAllRolesWithPermissions
{
    using MediatR;

    public record GetAllRolesWithPermissionsQuery : IRequest<List<GetAllRolesWithPermissionsResult>>;
}
