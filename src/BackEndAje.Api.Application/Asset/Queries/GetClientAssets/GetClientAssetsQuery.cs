namespace BackEndAje.Api.Application.Asset.Queries.GetClientAssets
{
    using BackEndAje.Api.Application.Abstractions.Common;
    using MediatR;

    public record GetClientAssetsQuery(
        int? PageNumber = null,
        int? PageSize = null,
        string? CodeAje = null,
        int? ClientId = null,
        int? userId = null,
        int? CediId = null,
        int? RegionId = null,
        int? Route = null,
        int? ClientCode = null) : IRequest<PaginatedResult<GetClientAssetsResult>>;
}