namespace BackEndAje.Api.Application.Census.Queries.GetAnswerByClientId
{
    using BackEndAje.Api.Application.Abstractions.Common;
    using BackEndAje.Api.Domain.Entities;
    using MediatR;

    public record GetAnswerByClientIdQuery(int? PageNumber = null, int? PageSize = null, int? AssetId = null, int? ClientId = null, string? MonthPeriod = null) : IRequest<PaginatedResult<ClientAssetWithCensusAnswersDto>>;
}