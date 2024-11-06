namespace BackEndAje.Api.Application.Asset.Queries.GetClientAssets
{
    using MediatR;

    public record GetClientAssetsQuery(string? CodeAje, int? ClientId) : IRequest<IEnumerable<GetClientAssetsResult>>;
}