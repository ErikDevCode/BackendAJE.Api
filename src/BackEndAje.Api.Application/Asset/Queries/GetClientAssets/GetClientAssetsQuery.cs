namespace BackEndAje.Api.Application.Asset.Queries.GetClientAssets
{
    using BackEndAje.Api.Application.Abstractions.Common;
    using MediatR;

    public record GetClientAssetsQuery(int PageNumber = 1, int PageSize = 10, string? CodeAje = null, int? ClientId = null, int? userId = null) : IRequest<PaginatedResult<GetClientAssetsResult>>;
}