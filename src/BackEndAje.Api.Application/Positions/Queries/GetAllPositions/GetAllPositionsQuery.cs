namespace BackEndAje.Api.Application.Positions.Queries.GetAllPositions
{
    using BackEndAje.Api.Application.Abstractions.Common;
    using MediatR;

    public record GetAllPositionsQuery(int PageNumber = 1, int PageSize = 10) : IRequest<PaginatedResult<GetAllPositionsResult>>;
}
