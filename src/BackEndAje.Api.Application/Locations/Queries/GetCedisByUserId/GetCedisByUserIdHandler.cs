namespace BackEndAje.Api.Application.Locations.Queries.GetCedisByUserId
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetCedisByUserIdHandler : IRequestHandler<GetCedisByUserIdQuery, List<GetCedisByUserIdResult>>
    {
        private readonly ICediRepository _cediRepository;
        private readonly IMapper _mapper;

        public GetCedisByUserIdHandler(ICediRepository cediRepository, IMapper mapper)
        {
            this._cediRepository = cediRepository;
            this._mapper = mapper;
        }

        public async Task<List<GetCedisByUserIdResult>> Handle(GetCedisByUserIdQuery request, CancellationToken cancellationToken)
        {
            var cedis = await this._cediRepository.GetCedisByUserIdAsync(request.userId);
            var result = this._mapper.Map<List<GetCedisByUserIdResult>>(cedis);
            return result;
        }
    }
}