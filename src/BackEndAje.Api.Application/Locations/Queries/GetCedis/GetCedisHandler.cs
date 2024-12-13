namespace BackEndAje.Api.Application.Locations.Queries.GetCedis
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetCedisHandler : IRequestHandler<GetCedisQuery, List<GetCedisResult>>
    {
        private readonly ICediRepository _cediRepository;
        private readonly IMapper _mapper;

        public GetCedisHandler(ICediRepository cediRepository, IMapper mapper)
        {
            this._cediRepository = cediRepository;
            this._mapper = mapper;
        }

        public async Task<List<GetCedisResult>> Handle(GetCedisQuery request, CancellationToken cancellationToken)
        {
            var cedi = await this._cediRepository.GetAllCedis();
            return this._mapper.Map<List<GetCedisResult>>(cedi);
        }
    }
}
