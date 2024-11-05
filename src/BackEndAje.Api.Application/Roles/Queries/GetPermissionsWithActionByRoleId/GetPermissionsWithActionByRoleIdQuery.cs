namespace BackEndAje.Api.Application.Roles.Queries.GetPermissionsWithActionByRoleId
{
    using MediatR;

    public record GetPermissionsWithActionByRoleIdQuery(int RoleId) : IRequest<List<GetPermissionsWithActionByRoleIdResult>>;
}

