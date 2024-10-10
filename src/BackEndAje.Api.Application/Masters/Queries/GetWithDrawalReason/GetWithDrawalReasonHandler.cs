namespace BackEndAje.Api.Application.Masters.Queries.GetWithDrawalReason
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetWithDrawalReasonHandler : IRequestHandler<GetWithDrawalReasonQuery, List<GetWithDrawalReasonResult>>
    {
        private readonly IMastersRepository _mastersRepository;
        private readonly IMapper _mapper;

        public GetWithDrawalReasonHandler(IMastersRepository mastersRepository, IMapper mapper)
        {
            this._mastersRepository = mastersRepository;
            this._mapper = mapper;
        }

        public async Task<List<GetWithDrawalReasonResult>> Handle(GetWithDrawalReasonQuery request, CancellationToken cancellationToken)
        {
            var reasonRequests = await this._mastersRepository.GetWithDrawalReasonsByReasonRequestId(request.ReasonRequestId);
            var result = this._mapper.Map<List<GetWithDrawalReasonResult>>(reasonRequests);
            return result;
        }
    }
}
