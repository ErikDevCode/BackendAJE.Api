namespace BackEndAje.Api.Application.Census.Queries.GetCensusQuestions
{
    public class GetCensusQuestionsResult
    {
        public int CensusQuestionsId { get; set; }

        public string Question { get; set; }

        public bool IsActive { get; set; }
    }
}