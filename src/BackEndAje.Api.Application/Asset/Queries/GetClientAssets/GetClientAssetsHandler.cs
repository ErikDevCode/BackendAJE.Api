namespace BackEndAje.Api.Application.Asset.Queries.GetClientAssets
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetClientAssetsHandler : IRequestHandler<GetClientAssetsQuery, IEnumerable<GetClientAssetsResult>>
    {
        private readonly IAssetRepository _assetRepository;
        private readonly IMapper _mapper;

        public GetClientAssetsHandler(IAssetRepository assetRepository, IMapper mapper)
        {
            this._assetRepository = assetRepository;
            this._mapper = mapper;
        }

        public async Task<IEnumerable<GetClientAssetsResult>> Handle(GetClientAssetsQuery request, CancellationToken cancellationToken)
        {
            var clientAssets = await this._assetRepository.GetClientAssetsAsync(request.CodeAje, request.ClientId);
            return this._mapper.Map<List<GetClientAssetsResult>>(clientAssets);
        }
    }
}

