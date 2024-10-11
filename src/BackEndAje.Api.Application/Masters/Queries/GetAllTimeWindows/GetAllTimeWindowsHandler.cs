namespace BackEndAje.Api.Application.Masters.Queries.GetAllTimeWindows
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetAllTimeWindowsHandler : IRequestHandler<GetAllTimeWindowsQuery, List<GetAllTimeWindowsResult>>
    {
        private readonly IMastersRepository _mastersRepository;
        private readonly IMapper _mapper;

        public GetAllTimeWindowsHandler(IMastersRepository mastersRepository, IMapper mapper)
        {
            this._mastersRepository = mastersRepository;
            this._mapper = mapper;
        }

        public async Task<List<GetAllTimeWindowsResult>> Handle(GetAllTimeWindowsQuery request, CancellationToken cancellationToken)
        {
            var timeWindows = await this._mastersRepository.GetAllTimeWindows();
            var result = this._mapper.Map<List<GetAllTimeWindowsResult>>(timeWindows);
            return result;
        }
    }
}
