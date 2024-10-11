namespace BackEndAje.Api.Application.Masters.Queries.GetAllProductSize
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetAllProductSizeHandler : IRequestHandler<GetAllProductSizeQuery, List<GetAllProductSizeResult>>
    {
        private readonly IMastersRepository _mastersRepository;
        private readonly IMapper _mapper;


        public GetAllProductSizeHandler(IMastersRepository mastersRepository, IMapper mapper)
        {
            this._mastersRepository = mastersRepository;
            this._mapper = mapper;
        }

        public async Task<List<GetAllProductSizeResult>> Handle(GetAllProductSizeQuery request, CancellationToken cancellationToken)
        {
            var productSizes = await this._mastersRepository.GetAllProductSize();
            var result = this._mapper.Map<List<GetAllProductSizeResult>>(productSizes);
            return result;
        }
    }
}
