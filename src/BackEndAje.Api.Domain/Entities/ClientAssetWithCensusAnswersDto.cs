namespace BackEndAje.Api.Domain.Entities
{
    public class ClientAssetWithCensusAnswersDto
    {
        public int ClientAssetId { get; set; }
        public int ClientId { get; set; }
        public int AssetId { get; set; }
        public int CediId { get; set; }
        public bool? IsActive { get; set; }
        public string CodeAje { get; set; }
        public string Notes { get; set; }
        public List<CensusAnswerDto> CensusAnswers { get; set; }
    }
}
