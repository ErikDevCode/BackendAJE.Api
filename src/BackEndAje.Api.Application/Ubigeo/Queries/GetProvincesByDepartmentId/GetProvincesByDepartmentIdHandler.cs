namespace BackEndAje.Api.Application.Ubigeo.Queries.GetProvincesByDepartmentId
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetProvincesByDepartmentIdHandler : IRequestHandler<GetProvincesByDepartmentIdQuery, List<GetProvincesByDepartmentIdResult>>
    {
        private readonly IUbigeoRepository _ubigeoRepository;
        private readonly IMapper _mapper;

        public GetProvincesByDepartmentIdHandler(IUbigeoRepository ubigeoRepository, IMapper mapper)
        {
            this._ubigeoRepository = ubigeoRepository;
            this._mapper = mapper;
        }

        public async Task<List<GetProvincesByDepartmentIdResult>> Handle(GetProvincesByDepartmentIdQuery request, CancellationToken cancellationToken)
        {
            var provinces = await this._ubigeoRepository.GetProvincesByDepartmentId(request.departmentId);
            var result = this._mapper.Map<List<GetProvincesByDepartmentIdResult>>(provinces);
            return result;
        }
    }
}
