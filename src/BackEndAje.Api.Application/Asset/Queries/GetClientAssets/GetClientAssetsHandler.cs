namespace BackEndAje.Api.Application.Asset.Queries.GetClientAssets
{
    using AutoMapper;
    using BackEndAje.Api.Application.Abstractions.Common;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetClientAssetsHandler : IRequestHandler<GetClientAssetsQuery, PaginatedResult<GetClientAssetsResult>>
    {
        private readonly IClientAssetRepository _clientAssetRepository;
        private readonly IMapper _mapper;

        public GetClientAssetsHandler(IMapper mapper, IClientAssetRepository clientAssetRepository)
        {
            this._mapper = mapper;
            this._clientAssetRepository = clientAssetRepository;
        }

        public async Task<PaginatedResult<GetClientAssetsResult>> Handle(GetClientAssetsQuery request, CancellationToken cancellationToken)
        {
            var clientAssets = await this._clientAssetRepository.GetClientAssetsAsync(request.PageNumber, request.PageSize, request.CodeAje, request.ClientId, request.userId);
            var result = this._mapper.Map<List<GetClientAssetsResult>>(clientAssets);

            var totalAssets = await this._clientAssetRepository.GetTotalClientAssets(request.CodeAje, request.ClientId);
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

