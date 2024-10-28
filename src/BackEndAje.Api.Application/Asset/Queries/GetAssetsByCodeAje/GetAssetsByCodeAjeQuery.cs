namespace BackEndAje.Api.Application.Asset.Queries.GetAssetsByCodeAje
{
    using MediatR;

    public record GetAssetsByCodeAjeQuery(string CodeAje) : IRequest<IEnumerable<GetAssetsByCodeAjeResult>>;
}

