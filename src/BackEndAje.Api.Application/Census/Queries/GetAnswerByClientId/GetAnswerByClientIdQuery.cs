namespace BackEndAje.Api.Application.Census.Queries.GetAnswerByClientId
{
    using BackEndAje.Api.Application.Abstractions.Common;
    using MediatR;

    public record GetAnswerByClientIdQuery(int? PageNumber = null, int? PageSize = null, int? ClientId = null, string? MonthPeriod = null) : IRequest<PaginatedResult<GetAnswerByClientIdResult>>;
}