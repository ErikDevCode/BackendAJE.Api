namespace BackEndAje.Api.Domain.Entities
{
    public class CensusQuestion
    {
        public int CensusQuestionsId { get; set; }

        public string Question { get; set; }

        public bool IsActive { get; set; }
    }
}