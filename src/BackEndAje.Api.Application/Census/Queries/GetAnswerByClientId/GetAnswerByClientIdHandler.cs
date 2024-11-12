namespace BackEndAje.Api.Application.Census.Queries.GetAnswerByClientId
{
    using AutoMapper;
    using BackEndAje.Api.Application.Abstractions.Common;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetAnswerByClientIdHandler : IRequestHandler<GetAnswerByClientIdQuery, PaginatedResult<GetAnswerByClientIdResult>>
    {
        private readonly ICensusRepository _censusRepository;
        private readonly IMapper _mapper;

        public GetAnswerByClientIdHandler(ICensusRepository censusRepository, IMapper mapper)
        {
            this._censusRepository = censusRepository;
            this._mapper = mapper;
        }

        public async Task<PaginatedResult<GetAnswerByClientIdResult>> Handle(GetAnswerByClientIdQuery request, CancellationToken cancellationToken)
        {
            var currentMonthPeriod = request.MonthPeriod ?? DateTime.Now.ToString("yyyyMM");

            var censusAnswers = await this._censusRepository.GetCensusAnswers(
                request.PageNumber,
                request.PageSize,
                request.ClientId,
                currentMonthPeriod);

            var result = this._mapper.Map<List<GetAnswerByClientIdResult>>(censusAnswers);

            var totalAnswers = await this._censusRepository.GetTotalCensusAnswers(request.ClientId, currentMonthPeriod);
            var paginatedResult = new PaginatedResult<GetAnswerByClientIdResult>
            {
                TotalCount = totalAnswers,
                PageNumber = request.PageNumber ?? 1,
                PageSize = request.PageSize ?? totalAnswers,
                Items = result,
            };

            return paginatedResult;
        }
    }
}