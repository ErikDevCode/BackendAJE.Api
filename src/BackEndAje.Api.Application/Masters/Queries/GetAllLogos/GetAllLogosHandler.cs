namespace BackEndAje.Api.Application.Masters.Queries.GetAllLogos
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetAllLogosHandler : IRequestHandler<GetAllLogosQuery, List<GetAllLogosResult>>
    {
        private readonly IMastersRepository _mastersRepository;
        private readonly IMapper _mapper;

        public GetAllLogosHandler(IMastersRepository mastersRepository, IMapper mapper)
        {
            this._mastersRepository = mastersRepository;
            this._mapper = mapper;
        }

        public async Task<List<GetAllLogosResult>> Handle(GetAllLogosQuery request, CancellationToken cancellationToken)
        {
            var logos = await this._mastersRepository.GetAllLogos();
            var result = this._mapper.Map<List<GetAllLogosResult>>(logos);
            return result;
        }
    }
}