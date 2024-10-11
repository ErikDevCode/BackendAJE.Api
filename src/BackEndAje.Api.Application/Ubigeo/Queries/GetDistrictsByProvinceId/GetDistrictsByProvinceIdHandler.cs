namespace BackEndAje.Api.Application.Ubigeo.Queries.GetDistrictsByProvinceId
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetDistrictsByProvinceIdHandler : IRequestHandler<GetDistrictsByProvinceIdQuery, List<GetDistrictsByProvinceIdResult>>
    {
        private readonly IUbigeoRepository _ubigeoRepository;
        private readonly IMapper _mapper;

        public GetDistrictsByProvinceIdHandler(IUbigeoRepository ubigeoRepository, IMapper mapper)
        {
            this._ubigeoRepository = ubigeoRepository;
            this._mapper = mapper;
        }

        public async Task<List<GetDistrictsByProvinceIdResult>> Handle(GetDistrictsByProvinceIdQuery request, CancellationToken cancellationToken)
        {
            var districts = await this._ubigeoRepository.GetDistrictsByProvinceId(request.provinceId);
            var result = this._mapper.Map<List<GetDistrictsByProvinceIdResult>>(districts);
            return result;
        }
    }
}
