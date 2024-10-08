namespace BackEndAje.Api.Application.Roles.Queries.GetAllRoles
{
    using BackEndAje.Api.Application.Abstractions.Common;
    using MediatR;

    public record GetAllRolesQuery(int PageNumber = 1, int PageSize = 10) : IRequest<PaginatedResult<GetAllRolesResult>>;
}
