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

            return await this._context.CensusQuestions.AsNoTracking().ToListAsync();
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
                from ca in this._context.CensusAnswer.AsNoTracking()
                join cq in this._context.CensusQuestions.AsNoTracking() on ca.CensusQuestionsId equals cq.CensusQuestionsId
                join c in this._context.Clients.AsNoTracking() on ca.ClientId equals c.ClientId
                join a in this._context.Assets.AsNoTracking() on ca.AssetId equals a.AssetId
                join u in this._context.Users.AsNoTracking() on ca.CreatedBy equals u.UserId into interviewer
                from intv in interviewer.DefaultIfEmpty()
                where (!clientId.HasValue || ca.ClientId == clientId.Value) && ca.MonthPeriod == currentMonthPeriod
                select new CensusAnswerDto
                {
                    CensusAnswerId = ca.CensusAnswerId,
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

        public async Task<(List<ClientAssetWithCensusAnswersDto> Items, int TotalCount)> GetClientAssetsWithCensusAnswersAsync(
            int? pageNumber,
            int? pageSize,
            int? assetId,
            int? clientId,
            string? monthPeriod)
        {
            var currentMonthPeriod = monthPeriod ?? DateTime.Now.ToString("yyyyMM");

            var query =
                from ca in this._context.CensusAnswer.AsNoTracking()
                join cq in this._context.CensusQuestions.AsNoTracking() on ca.CensusQuestionsId equals cq.CensusQuestionsId
                join cl in this._context.Clients.AsNoTracking() on ca.ClientId equals cl.ClientId
                join a in this._context.Assets.AsNoTracking() on ca.AssetId equals a.AssetId
                join clientAsset in this._context.ClientAssets.AsNoTracking() on new { ca.ClientId, ca.AssetId } equals new { clientAsset.ClientId, clientAsset.AssetId }
                join u in this._context.Users.AsNoTracking() on ca.CreatedBy equals u.UserId into interviewer
                from intv in interviewer.DefaultIfEmpty()
                where
                    (!clientId.HasValue || ca.ClientId == clientId.Value) &&
                    (!assetId.HasValue || ca.AssetId == assetId.Value) &&
                    ca.MonthPeriod == currentMonthPeriod
                select new
                {
                    clientAsset.ClientAssetId,
                    clientAsset.CediId,
                    clientAsset.Notes,
                    clientAsset.InstallationDate,
                    ClientAsset = new
                    {
                        clientAsset.ClientAssetId,
                        clientAsset.ClientId,
                        clientAsset.AssetId,
                        clientAsset.CediId,
                        clientAsset.IsActive,
                        clientAsset.CodeAje,
                        clientAsset.Notes,
                    },
                    CensusAnswer = new CensusAnswerDto
                    {
                        CensusAnswerId = ca.CensusAnswerId,
                        CensusQuestionsId = ca.CensusQuestionsId,
                        Question = cq.Question,
                        Answer = ca.Answer,
                        ClientId = ca.ClientId,
                        ClientName = cl.CompanyName,
                        AssetId = ca.AssetId,
                        AssetName = $"{a.Logo} {a.Brand} {a.Model}",
                        MonthPeriod = ca.MonthPeriod,
                        CreatedBy = ca.CreatedBy,
                        InterviewerName = intv != null ? $"{intv.Names} {intv.PaternalSurName} {intv.MaternalSurName}" : null,
                        CreatedAt = ca.CreatedAt,
                    },
                };

            // Calcular total antes de aplicar la paginación
            var totalCount = await query
                .GroupBy(x => x.ClientAsset)
                .CountAsync();

            var groupedQuery = query
                .GroupBy(x => x.ClientAsset)
                .Select(g => new ClientAssetWithCensusAnswersDto
                {
                    ClientAssetId = g.Key.ClientAssetId,
                    ClientId = g.Key.ClientId,
                    AssetId = g.Key.AssetId,
                    CediId = g.Key.CediId,
                    IsActive = g.Key.IsActive,
                    CodeAje = g.Key.CodeAje,
                    Notes = g.Key.Notes,
                    CensusAnswers = g.Select(x => x.CensusAnswer).ToList(),
                });

            // Aplicar paginación
            if (pageNumber.HasValue && pageSize.HasValue && pageNumber > 0 && pageSize > 0)
            {
                groupedQuery = groupedQuery
                    .Skip((pageNumber.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value);
            }

            var items = await groupedQuery.ToListAsync();

            return (items, totalCount);
        }

        public async Task<CensusAnswer?> GetCensusAnswerAsync(int censusQuestionsId, int clientId, int assetId, string monthPeriod)
        {
            return await this._context.CensusAnswer
                .FirstOrDefaultAsync(ca =>
                    ca.CensusQuestionsId == censusQuestionsId &&
                    ca.ClientId == clientId &&
                    ca.AssetId == assetId &&
                    ca.MonthPeriod == monthPeriod);
        }

        public async Task<CensusAnswer?> GetCensusAnswerById(int censusAnswerId)
        {
            return await this._context.CensusAnswer.AsNoTracking().FirstOrDefaultAsync(x => x.CensusAnswerId == censusAnswerId);
        }

        public async Task UpdateCensusAnswer(CensusAnswer censusAnswer)
        {
            this._context.Entry(censusAnswer).State = EntityState.Detached;
            this._context.CensusAnswer.Update(censusAnswer);
            await this._context.SaveChangesAsync();
        }

        public async Task<int> GetTotalCensusAnswers(int? clientId, string? monthPeriod)
        {
            var query = this._context.CensusAnswer.AsNoTracking().AsQueryable();

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

        public async Task<int> GetCensusCountAsync(
            int? regionId,
            int? cediId,
            int? zoneId,
            int? route,
            int? month,
            int? year)
        {
            var query =
                from ca in this._context.CensusAnswer
                join cl in this._context.Clients on ca.ClientId equals cl.ClientId
                join a in this._context.Assets on ca.AssetId equals a.AssetId
                join clientAsset in this._context.ClientAssets
                    on new { ca.ClientId, ca.AssetId } equals new { clientAsset.ClientId, clientAsset.AssetId }
                join u in this._context.Users on cl.UserId equals u.UserId
                join cedi in this._context.Cedis on u.CediId equals cedi.CediId into cediGroup
                from cedi in cediGroup.DefaultIfEmpty()
                join zone in this._context.Zones on cedi.CediId equals zone.CediId into zoneGroup
                from zone in zoneGroup.DefaultIfEmpty()
                where
                    (regionId == null || (cedi != null && cedi.RegionId == regionId.Value)) &&
                    (cediId == null || (cedi != null && cedi.CediId == cediId.Value)) &&
                    (zoneId == null || (zone != null && zone.ZoneId == zoneId.Value)) &&
                    (!route.HasValue || cl.Route == route.Value) &&
                    (month == null || (ca.MonthPeriod.Substring(4, 2) == month.Value.ToString("D2"))) &&
                    (!year.HasValue || ca.MonthPeriod.StartsWith(year.Value.ToString()))
                group ca by new { ca.ClientId, ca.AssetId, ca.MonthPeriod } into groupedCensus
                select groupedCensus.Key;

            var censusCount = await query.CountAsync();

            return censusCount;
        }
    }
}