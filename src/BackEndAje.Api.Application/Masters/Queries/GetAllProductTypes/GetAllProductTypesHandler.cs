namespace BackEndAje.Api.Application.Masters.Queries.GetAllProductTypes
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetAllProductTypesHandler : IRequestHandler<GetAllProductTypesQuery, List<GetAllProductTypesResult>>
    {
        private readonly IMastersRepository _mastersRepository;
        private readonly IMapper _mapper;

        public GetAllProductTypesHandler(IMastersRepository mastersRepository, IMapper mapper)
        {
            this._mastersRepository = mastersRepository;
            this._mapper = mapper;
        }

        public async Task<List<GetAllProductTypesResult>> Handle(GetAllProductTypesQuery request, CancellationToken cancellationToken)
        {
            var productTypes = await this._mastersRepository.GetAllProductTypes();
            var result = this._mapper.Map<List<GetAllProductTypesResult>>(productTypes);
            return result;
        }
    }
}
