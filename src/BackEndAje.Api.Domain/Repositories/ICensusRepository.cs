using BackEndAje.Api.Domain.Entities;

namespace BackEndAje.Api.Domain.Repositories
{
    public interface ICensusRepository
    {
        Task<List<CensusQuestion>> GetCensusQuestions(int clientId);

        Task AddCensusAnswer(CensusAnswer censusAnswer);
        
        Task<List<CensusAnswerDto>> GetCensusAnswers(int? pageNumber, int? pageSize, int? clientId, string? monthPeriod);
        Task<int> GetTotalCensusAnswers(int? clientId, string? monthPeriod);
    }
}