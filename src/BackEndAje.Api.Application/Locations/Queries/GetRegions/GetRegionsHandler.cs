namespace BackEndAje.Api.Application.Locations.Queries.GetRegions
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetRegionsHandler : IRequestHandler<GetRegionsQuery, List<GetRegionsResult>>
    {
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;

        public GetRegionsHandler(IRegionRepository regionRepository, IMapper mapper)
        {
            this._regionRepository = regionRepository;
            this._mapper = mapper;
        }

        public async Task<List<GetRegionsResult>> Handle(GetRegionsQuery request, CancellationToken cancellationToken)
        {
            var regions = await this._regionRepository.GetAllRegionsAsync();
            var result = this._mapper.Map<List<GetRegionsResult>>(regions);
            return result;
        }
    }
}
