namespace BackEndAje.Api.Application.Locations.Queries.GetCedisByRegionId
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetCedisByRegionIdHandler : IRequestHandler<GetCedisByRegionIdQuery, List<GetCedisByRegionIdResult>>
    {
        private readonly ICediRepository _cediRepository;
        private readonly IMapper _mapper;

        public GetCedisByRegionIdHandler(ICediRepository cediRepository, IMapper mapper)
        {
            this._cediRepository = cediRepository;
            this._mapper = mapper;
        }

        public async Task<List<GetCedisByRegionIdResult>> Handle(GetCedisByRegionIdQuery request, CancellationToken cancellationToken)
        {
            var cedis = await this._cediRepository.GetCedisByRegionIdAsync(request.RegionId);
            var result = this._mapper.Map<List<GetCedisByRegionIdResult>>(cedis);
            return result;
        }
    }
}
