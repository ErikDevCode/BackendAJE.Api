namespace BackEndAje.Api.Application.Locations.Queries.GetCedisById
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetCedisByIdHandler : IRequestHandler<GetCedisByIdQuery, GetCedisByIdResult>
    {
        private readonly ICediRepository _cediRepository;
        private readonly IMapper _mapper;

        public GetCedisByIdHandler(ICediRepository cediRepository, IMapper mapper)
        {
            this._cediRepository = cediRepository;
            this._mapper = mapper;
        }

        public async Task<GetCedisByIdResult> Handle(GetCedisByIdQuery request, CancellationToken cancellationToken)
        {
            var cedi = await this._cediRepository.GetCediByCediIdAsync(request.cediId);
            return this._mapper.Map<GetCedisByIdResult>(cedi);
        }
    }
}