namespace BackEndAje.Api.Application.Census.Queries.GetCensusQuestions
{
    using MediatR;

    public record GetCensusQuestionsQuery : IRequest<List<GetCensusQuestionsResult>>;
}