namespace BackEndAje.Api.Application.Asset.Queries.GetAllAssets
{
    using BackEndAje.Api.Application.Abstractions.Common;
    using MediatR;

    public record GetAllAssetsQuery(int PageNumber = 1, int PageSize = 10, string? CodeAje = null) : IRequest<PaginatedResult<GetAllAssetsResult>>;
}