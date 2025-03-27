namespace BackEndAje.Api.Domain.Entities
{
    public class CensusAnswer
    {
        public int CensusAnswerId { get; set; }
        
        public int CensusFormId { get; set; }
        
        public int? ClientAssetId { get; set; }
        public int CensusQuestionsId { get; set; }
        public string Answer { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
    }
}