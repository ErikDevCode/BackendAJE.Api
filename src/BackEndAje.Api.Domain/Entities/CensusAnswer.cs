namespace BackEndAje.Api.Domain.Entities
{
    public class CensusAnswer
    {
        public int CensusAnswerId { get; set; }
        public int CensusQuestionsId { get; set; }
        public string Answer { get; set; }
        public int ClientId { get; set; }
        public string MonthPeriod { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
    }
}