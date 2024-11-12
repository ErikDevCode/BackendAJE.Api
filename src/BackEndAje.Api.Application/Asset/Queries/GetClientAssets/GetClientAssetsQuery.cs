namespace BackEndAje.Api.Application.Asset.Queries.GetClientAssets
{
    using BackEndAje.Api.Application.Abstractions.Common;
    using MediatR;

    public record GetClientAssetsQuery(int? PageNumber = null, int? PageSize = null, string? CodeAje = null, int? ClientId = null, int? userId = null) : IRequest<PaginatedResult<GetClientAssetsResult>>;
}