namespace BackEndAje.Api.Application.Asset.Queries.GetAssetWithOutClient
{
    using AutoMapper;
    using BackEndAje.Api.Application.Abstractions.Common;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetAssetWithOutClientHandler : IRequestHandler<GetAssetWithOutClientQuery, PaginatedResult<GetAssetWithOutClientResult>>
    {
        private readonly IAssetRepository _assetRepository;
        private readonly IMapper _mapper;

        public GetAssetWithOutClientHandler(IAssetRepository assetRepository, IMapper mapper)
        {
            this._assetRepository = assetRepository;
            this._mapper = mapper;
        }

        public async Task<PaginatedResult<GetAssetWithOutClientResult>> Handle(GetAssetWithOutClientQuery request, CancellationToken cancellationToken)
        {
            var assets = await this._assetRepository.GetAssetsWithOutClient(request.PageNumber, request.PageSize, request.CodeAje);

            var result = this._mapper.Map<List<GetAssetWithOutClientResult>>(assets);
            var totalAssets = await this._assetRepository.GetTotalAssetsWithOutClient(request.CodeAje);

            var paginatedResult = new PaginatedResult<GetAssetWithOutClientResult>
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

