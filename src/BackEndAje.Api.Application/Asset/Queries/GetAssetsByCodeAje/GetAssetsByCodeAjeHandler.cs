namespace BackEndAje.Api.Application.Asset.Queries.GetAssetsByCodeAje
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetAssetsByCodeAjeHandler : IRequestHandler<GetAssetsByCodeAjeQuery, IEnumerable<GetAssetsByCodeAjeResult>>
    {
        private readonly IAssetRepository _assetRepository;
        private readonly IMapper _mapper;

        public GetAssetsByCodeAjeHandler(IAssetRepository assetRepository, IMapper mapper)
        {
            this._assetRepository = assetRepository;
            this._mapper = mapper;
        }

        public async Task<IEnumerable<GetAssetsByCodeAjeResult>> Handle(GetAssetsByCodeAjeQuery request, CancellationToken cancellationToken)
        {
            var assets = await this._assetRepository.GetAssetByCodeAje(request.CodeAje);

            if (!assets.Any())
            {
                throw new KeyNotFoundException($"Activo con código {request.CodeAje} no encontrado.");
            }

            var result = this._mapper.Map<List<GetAssetsByCodeAjeResult>>(assets);
            return result;
        }
    }
}