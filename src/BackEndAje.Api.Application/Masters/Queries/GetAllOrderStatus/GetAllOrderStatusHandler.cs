namespace BackEndAje.Api.Application.Masters.Queries.GetAllOrderStatus
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetAllOrderStatusHandler : IRequestHandler<GetAllOrderStatusQuery, List<GetAllOrderStatusResult>>
    {
        private readonly IMastersRepository _mastersRepository;
        private readonly IMapper _mapper;

        public GetAllOrderStatusHandler(IMastersRepository mastersRepository, IMapper mapper)
        {
            this._mastersRepository = mastersRepository;
            this._mapper = mapper;
        }

        public async Task<List<GetAllOrderStatusResult>> Handle(GetAllOrderStatusQuery request, CancellationToken cancellationToken)
        {
            var logos = await this._mastersRepository.GetAllOrderStatus(request.userId);
            var result = this._mapper.Map<List<GetAllOrderStatusResult>>(logos);
            return result;
        }
    }
}