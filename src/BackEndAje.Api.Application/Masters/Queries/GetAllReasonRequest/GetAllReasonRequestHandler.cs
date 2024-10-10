namespace BackEndAje.Api.Application.Masters.Queries.GetAllReasonRequest
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetAllReasonRequestHandler : IRequestHandler<GetAllReasonRequestQuery, List<GetAllReasonRequestResult>>
    {
        private readonly IMastersRepository _mastersRepository;
        private readonly IMapper _mapper;

        public GetAllReasonRequestHandler(IMastersRepository mastersRepository, IMapper mapper)
        {
            this._mastersRepository = mastersRepository;
            this._mapper = mapper;
        }

        public async Task<List<GetAllReasonRequestResult>> Handle(GetAllReasonRequestQuery request, CancellationToken cancellationToken)
        {
            var reasonRequests = await this._mastersRepository.GetAllReasonRequest();
            var result = this._mapper.Map<List<GetAllReasonRequestResult>>(reasonRequests);
            return result;
        }
    }
}
