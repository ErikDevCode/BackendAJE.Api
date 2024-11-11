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

        public async Task<List<CensusQuestion>> GetCensusQuestions()
        {
            return await this._context.CensusQuestions.ToListAsync();
        }

        public async Task AddCensusAnswer(CensusAnswer censusAnswer)
        {
            this._context.CensusAnswer.Add(censusAnswer);
            await this._context.SaveChangesAsync();
        }
    }
}