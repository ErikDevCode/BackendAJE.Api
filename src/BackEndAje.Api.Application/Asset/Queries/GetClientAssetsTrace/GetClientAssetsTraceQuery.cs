namespace BackEndAje.Api.Application.Asset.Queries.GetClientAssetsTrace
{
    using BackEndAje.Api.Application.Abstractions.Common;
    using MediatR;

    public record GetClientAssetsTraceQuery(int PageNumber = 1, int PageSize = 10, int? AssetId = null)
        : IRequest<PaginatedResult<GetClientAssetsTraceResult>>;
}
