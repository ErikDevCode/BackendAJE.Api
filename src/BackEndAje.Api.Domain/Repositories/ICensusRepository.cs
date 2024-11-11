using BackEndAje.Api.Domain.Entities;

namespace BackEndAje.Api.Domain.Repositories
{
    public interface ICensusRepository
    {
        Task<List<CensusQuestion>> GetCensusQuestions(int clientId);

        Task AddCensusAnswer(CensusAnswer censusAnswer);
    }
}