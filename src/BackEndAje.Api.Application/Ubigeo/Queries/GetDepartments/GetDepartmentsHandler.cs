namespace BackEndAje.Api.Application.Ubigeo.Queries.GetDepartments
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetDepartmentsHandler : IRequestHandler<GetDepartmentsQuery, List<GetDepartmentsResult>>
    {
        private readonly IUbigeoRepository _ubigeoRepository;
        private readonly IMapper _mapper;

        public GetDepartmentsHandler(IUbigeoRepository ubigeoRepository, IMapper mapper)
        {
            this._ubigeoRepository = ubigeoRepository;
            this._mapper = mapper;
        }

        public async Task<List<GetDepartmentsResult>> Handle(GetDepartmentsQuery request, CancellationToken cancellationToken)
        {
            var departments = await this._ubigeoRepository.GetDepartments();
            var result = this._mapper.Map<List<GetDepartmentsResult>>(departments);
            return result;
        }
    }
}
