namespace BackEndAje.Api.Application.Users.Queries.GetAllUser
{
    using BackEndAje.Api.Application.Abstractions.Common;
    using MediatR;

    public record GetAllUserQuery(int PageNumber = 1, int PageSize = 10) : IRequest<PaginatedResult<GetAllUserResult>>;
}