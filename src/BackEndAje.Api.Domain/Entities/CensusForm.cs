namespace BackEndAje.Api.Domain.Entities
{
    public class CensusForm
    {
        public int CensusFormId { get; set; }
        
        public int ClientId { get; set; }
        
        public string MonthPeriod { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime UpdatedAt { get; set; }
    }
}
