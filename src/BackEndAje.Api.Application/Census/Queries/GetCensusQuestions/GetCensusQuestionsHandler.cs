namespace BackEndAje.Api.Application.Census.Queries.GetCensusQuestions
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetCensusQuestionsHandler : IRequestHandler<GetCensusQuestionsQuery, List<GetCensusQuestionsResult>>
    {
        private readonly ICensusRepository _censusRepository;
        private readonly IMapper _mapper;

        public GetCensusQuestionsHandler(ICensusRepository censusRepository, IMapper mapper)
        {
            this._censusRepository = censusRepository;
            this._mapper = mapper;
        }

        public async Task<List<GetCensusQuestionsResult>> Handle(GetCensusQuestionsQuery request, CancellationToken cancellationToken)
        {
            var censusQuestions = await this._censusRepository.GetCensusQuestions();
            var result = this._mapper.Map<List<GetCensusQuestionsResult>>(censusQuestions);
            return result;
        }
    }
}