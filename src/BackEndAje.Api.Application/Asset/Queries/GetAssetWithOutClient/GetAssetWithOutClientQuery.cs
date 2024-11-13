namespace BackEndAje.Api.Application.Asset.Queries.GetAssetWithOutClient
{
    using BackEndAje.Api.Application.Abstractions.Common;
    using MediatR;

    public record GetAssetWithOutClientQuery(int PageNumber = 1, int PageSize = 10, string? CodeAje = null) : IRequest<PaginatedResult<GetAssetWithOutClientResult>>;
}

