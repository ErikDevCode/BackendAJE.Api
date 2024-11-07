namespace BackEndAje.Api.Application.Asset.Queries.GetAllAssets
{
    using AutoMapper;
    using BackEndAje.Api.Application.Abstractions.Common;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetAllAssetsHandler : IRequestHandler<GetAllAssetsQuery, PaginatedResult<GetAllAssetsResult>>
    {
        private readonly IAssetRepository _assetRepository;
        private readonly IMapper _mapper;

        public GetAllAssetsHandler(IAssetRepository assetRepository, IMapper mapper)
        {
            this._assetRepository = assetRepository;
            this._mapper = mapper;
        }

        public async Task<PaginatedResult<GetAllAssetsResult>> Handle(GetAllAssetsQuery request, CancellationToken cancellationToken)
        {
            var assets = await this._assetRepository.GetAssets(request.PageNumber, request.PageSize, request.CodeAje);

            var result = this._mapper.Map<List<GetAllAssetsResult>>(assets);
            var totalAssets = await this._assetRepository.GetTotalAssets(request.CodeAje);

            var paginatedResult = new PaginatedResult<GetAllAssetsResult>
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
