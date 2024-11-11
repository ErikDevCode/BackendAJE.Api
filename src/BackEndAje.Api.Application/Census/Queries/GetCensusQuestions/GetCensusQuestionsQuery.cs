namespace BackEndAje.Api.Application.Census.Queries.GetCensusQuestions
{
    using MediatR;

    public record GetCensusQuestionsQuery(int clientId) : IRequest<List<GetCensusQuestionsResult>>;
}