namespace BackEndAje.Api.Infrastructure.Repositories
{
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using BackEndAje.Api.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;

    public class CensusRepository : ICensusRepository
    {
        private readonly ApplicationDbContext _context;

        public CensusRepository(ApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<List<CensusQuestion>> GetCensusQuestions(int clientId)
        {
            var currentMonthPeriod = DateTime.Now.ToString("yyyyMM");
            var hasCensusAnswers = await this._context.CensusAnswer
                .AnyAsync(ca => ca.ClientId == clientId && ca.MonthPeriod == currentMonthPeriod);
            if (hasCensusAnswers)
            {
                return null!;
            }

            return await this._context.CensusQuestions.ToListAsync();
        }

        public async Task AddCensusAnswer(CensusAnswer censusAnswer)
        {
            this._context.CensusAnswer.Add(censusAnswer);
            await this._context.SaveChangesAsync();
        }
    }
}