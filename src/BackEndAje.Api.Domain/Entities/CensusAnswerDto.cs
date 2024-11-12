namespace BackEndAje.Api.Domain.Entities
{
    public class CensusAnswerDto
    {
        public int CensusQuestionsId { get; set; }

        public string Question { get; set; }

        public string Answer { get; set; }

        public int ClientId { get; set; }

        public string ClientName { get; set; }

        public int AssetId { get; set; }

        public string AssetName { get; set; }

        public string MonthPeriod { get; set; }

        public int CreatedBy { get; set; }

        public string InterviewerName { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}

