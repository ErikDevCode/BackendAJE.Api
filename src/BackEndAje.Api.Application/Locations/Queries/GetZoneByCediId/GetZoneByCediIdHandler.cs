namespace BackEndAje.Api.Application.Locations.Queries.GetZoneByCediId
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetZoneByCediIdHandler : IRequestHandler<GetZoneByCediIdQuery, List<GetZoneByCediIdResult>>
    {
        private readonly IZoneRepository _zoneRepository;
        private readonly IMapper _mapper;

        public GetZoneByCediIdHandler(IZoneRepository zoneRepository, IMapper mapper)
        {
            this._zoneRepository = zoneRepository;
            this._mapper = mapper;
        }

        public async Task<List<GetZoneByCediIdResult>> Handle(GetZoneByCediIdQuery request, CancellationToken cancellationToken)
        {
            var zones = request.CediId.HasValue
                ? await this._zoneRepository.GetZonesByCediIdAsync(request.CediId.Value)
                : await this._zoneRepository.GetAllZones();
            var result = this._mapper.Map<List<GetZoneByCediIdResult>>(zones);
            return result;
        }
    }
}
