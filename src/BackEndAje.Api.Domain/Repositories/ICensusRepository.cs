using BackEndAje.Api.Domain.Entities;

namespace BackEndAje.Api.Domain.Repositories
{
    public interface ICensusRepository
    {
        Task<List<CensusQuestion>> GetCensusQuestions(int clientId);

        Task AddCensusAnswer(CensusAnswer censusAnswer);
        
        Task<List<CensusAnswerDto>> GetCensusAnswers(int? pageNumber, int? pageSize, int? clientId, string? monthPeriod);
        Task<int> GetTotalCensusAnswers(int? clientId, string? monthPeriod);

        Task<(List<ClientAssetWithCensusAnswersDto> Items, int TotalCount)> GetClientAssetsWithCensusAnswersAsync(int? pageNumber,
            int? pageSize, int? AssetId, int? clientId, string? monthPeriod);

        Task<CensusAnswer?> GetCensusAnswerAsync(int censusQuestionsId, int clientId, int assetId, string monthPeriod);

        Task<CensusAnswer?> GetCensusAnswerById(int censusAnswerId);
        
        Task UpdateCensusAnswer(CensusAnswer censusAnswer);

        Task<int> GetCensusCountAsync(
            int? regionId,
            int? cediId,
            int? zoneId,
            int? route,
            int? month,
            int? year);
    }
}