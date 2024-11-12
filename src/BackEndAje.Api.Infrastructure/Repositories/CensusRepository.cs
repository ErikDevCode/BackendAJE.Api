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

        public async Task<List<CensusAnswerDto>> GetCensusAnswers(int? pageNumber, int? pageSize, int? clientId, string? monthPeriod)
        {
            var currentMonthPeriod = monthPeriod ?? DateTime.Now.ToString("yyyyMM");

            var query =
                from ca in this._context.CensusAnswer
                join cq in this._context.CensusQuestions on ca.CensusQuestionsId equals cq.CensusQuestionsId
                join c in this._context.Clients on ca.ClientId equals c.ClientId
                join a in this._context.Assets on ca.AssetId equals a.AssetId
                join u in this._context.Users on ca.CreatedBy equals u.UserId into interviewer
                from intv in interviewer.DefaultIfEmpty()
                where (!clientId.HasValue || ca.ClientId == clientId.Value) && ca.MonthPeriod == currentMonthPeriod
                select new CensusAnswerDto
                {
                    CensusQuestionsId = ca.CensusQuestionsId,
                    Question = cq.Question,
                    Answer = ca.Answer,
                    ClientId = ca.ClientId,
                    ClientName = c.CompanyName,
                    AssetId = ca.AssetId,
                    AssetName = $"{a.Logo} {a.Brand} {a.Model}",
                    MonthPeriod = ca.MonthPeriod,
                    CreatedBy = ca.CreatedBy,
                    InterviewerName = intv != null ? $"{intv.Names} {intv.PaternalSurName} {intv.MaternalSurName}" : null,
                    CreatedAt = ca.CreatedAt,
                };

            if (pageNumber.HasValue && pageSize.HasValue && pageNumber > 0 && pageSize > 0)
            {
                query = query
                    .Skip((pageNumber.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<int> GetTotalCensusAnswers(int? clientId, string? monthPeriod)
        {
            var query = this._context.CensusAnswer.AsQueryable();

            if (clientId.HasValue)
            {
                query = query.Where(ca => ca.ClientId == clientId.Value);
            }

            if (!string.IsNullOrEmpty(monthPeriod))
            {
                query = query.Where(ca => ca.MonthPeriod == monthPeriod);
            }

            return await query.CountAsync();
        }
    }
}