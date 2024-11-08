namespace BackEndAje.Api.Application.Asset.Queries.GetClientAssets
{
    using AutoMapper;
    using BackEndAje.Api.Application.Abstractions.Common;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetClientAssetsHandler : IRequestHandler<GetClientAssetsQuery, PaginatedResult<GetClientAssetsResult>>
    {
        private readonly IAssetRepository _assetRepository;
        private readonly IMapper _mapper;

        public GetClientAssetsHandler(IAssetRepository assetRepository, IMapper mapper)
        {
            this._assetRepository = assetRepository;
            this._mapper = mapper;
        }

        public async Task<PaginatedResult<GetClientAssetsResult>> Handle(GetClientAssetsQuery request, CancellationToken cancellationToken)
        {
            var clientAssets = await this._assetRepository.GetClientAssetsAsync(request.CodeAje, request.ClientId);
            var result = this._mapper.Map<List<GetClientAssetsResult>>(clientAssets);

            var totalAssets = await this._assetRepository.GetTotalClientAssets(request.CodeAje, request.ClientId);
            var paginatedResult = new PaginatedResult<GetClientAssetsResult>
            {
                TotalCount = totalAssets,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Items = result,
            };

            return paginatedResult;
        }
    }
}

