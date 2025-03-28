using BackEndAje.Api.Application.Dtos.Const;

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
            var hasCensusForm = await this._context.CensusForm
                .AnyAsync(cf => cf.ClientId == clientId && cf.MonthPeriod == currentMonthPeriod);

            if (hasCensusForm)
            {
                return null!;
            }

            return await this._context.CensusQuestions
                .AsNoTracking()
                .Where(q => q.IsActive)
                .ToListAsync();
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
                join cf in this._context.CensusForm.AsNoTracking() on ca.CensusFormId equals cf.CensusFormId
                join cl in this._context.Clients.AsNoTracking() on cf.ClientId equals cl.ClientId
                join clientAsset in this._context.ClientAssets.AsNoTracking() on ca.ClientAssetId equals clientAsset.ClientAssetId into clientAssetGroup
                from clientAsset in clientAssetGroup.DefaultIfEmpty()
                join asset in this._context.Assets.AsNoTracking() on clientAsset.AssetId equals asset.AssetId into assetGroup
                from asset in assetGroup.DefaultIfEmpty()
                join u in this._context.Users.AsNoTracking() on ca.CreatedBy equals u.UserId into interviewer
                from intv in interviewer.DefaultIfEmpty()
                where (!clientId.HasValue || cf.ClientId == clientId.Value) &&
                      cf.MonthPeriod == currentMonthPeriod
                select new
                {
                    ca,
                    cq,
                    cf,
                    cl,
                    clientAsset,
                    asset,
                    intv
                };

            var result = await query.ToListAsync();

            var finalResult = result.Select(x => new CensusAnswerDto
            {
                CensusAnswerId = x.ca.CensusAnswerId,
                CensusQuestionsId = x.ca.CensusQuestionsId,
                Question = x.cq.Question,
                Answer = x.ca.Answer,
                ClientId = x.cf.ClientId,
                ClientName = x.cl.CompanyName,
                AssetId = x.clientAsset != null ? x.clientAsset.AssetId : 0,
                AssetName = x.asset != null ? $"{x.asset.Logo} {x.asset.Brand} {x.asset.Model}" : null,
                MonthPeriod = x.cf.MonthPeriod,
                CreatedBy = x.ca.CreatedBy,
                InterviewerName = x.intv != null ? $"{x.intv.Names} {x.intv.PaternalSurName} {x.intv.MaternalSurName}" : null,
                CreatedAt = x.ca.CreatedAt,
            });


            if (pageNumber.HasValue && pageSize.HasValue && pageNumber > 0 && pageSize > 0)
            {
                finalResult = finalResult
                    .Skip((pageNumber.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value);
            }

            return finalResult.ToList();
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
                join cf in this._context.CensusForm.AsNoTracking() on ca.CensusFormId equals cf.CensusFormId
                join cl in this._context.Clients.AsNoTracking() on cf.ClientId equals cl.ClientId
                join cq in this._context.CensusQuestions.AsNoTracking() on ca.CensusQuestionsId equals cq.CensusQuestionsId
                join clientAsset in this._context.ClientAssets.AsNoTracking() on ca.ClientAssetId equals clientAsset.ClientAssetId
                join a in this._context.Assets.AsNoTracking() on clientAsset.AssetId equals a.AssetId
                join u in this._context.Users.AsNoTracking() on ca.CreatedBy equals u.UserId into interviewer
                from intv in interviewer.DefaultIfEmpty()
                where cf.MonthPeriod == currentMonthPeriod &&
                      (!clientId.HasValue || cf.ClientId == clientId.Value) &&
                      (!assetId.HasValue || clientAsset.AssetId == assetId.Value)
                select new
                {
                    clientAsset,
                    CensusAnswer = new CensusAnswerDto
                    {
                        CensusAnswerId = ca.CensusAnswerId,
                        CensusQuestionsId = ca.CensusQuestionsId,
                        Question = cq.Question,
                        Answer = ca.Answer,
                        ClientId = cf.ClientId,
                        ClientName = cl.CompanyName,
                        AssetId = clientAsset.AssetId,
                        AssetName = $"{a.Logo} {a.Brand} {a.Model}",
                        MonthPeriod = cf.MonthPeriod,
                        CreatedBy = ca.CreatedBy,
                        InterviewerName = intv != null ? $"{intv.Names} {intv.PaternalSurName} {intv.MaternalSurName}" : null,
                        CreatedAt = ca.CreatedAt,
                    },
                };

            // Agrupamos por ClientAsset para el total
            var totalCount = await query
                .GroupBy(x => x.clientAsset.ClientAssetId)
                .CountAsync();

            var groupedQuery = query
                .GroupBy(x => x.clientAsset)
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
            var censusForm = await this._context.CensusForm
                .FirstOrDefaultAsync(cf => cf.ClientId == clientId && cf.MonthPeriod == monthPeriod);

            if (censusForm == null)
            {
                return null;
            }

            // Obtener el clientAsset que une ese cliente con ese asset
            var clientAsset = await this._context.ClientAssets
                .FirstOrDefaultAsync(ca => ca.ClientId == clientId && ca.AssetId == assetId);

            if (clientAsset == null)
            {
                return null;
            }

            // Buscar la respuesta
            return await this._context.CensusAnswer
                .FirstOrDefaultAsync(ca =>
                    ca.CensusQuestionsId == censusQuestionsId &&
                    ca.CensusFormId == censusForm.CensusFormId &&
                    ca.ClientAssetId == clientAsset.ClientAssetId);
        }

        public async Task<CensusAnswer?> GetCensusAnswerAsync(int censusQuestionsId, int censusFormId, int? clientAssetId)
        {
            return await this._context.CensusAnswer
                .FirstOrDefaultAsync(ca =>
                    ca.CensusQuestionsId == censusQuestionsId &&
                    ca.CensusFormId == censusFormId &&
                    ca.ClientAssetId == clientAssetId);
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
            var query =
                from ca in this._context.CensusAnswer.AsNoTracking()
                join cf in this._context.CensusForm.AsNoTracking()
                    on ca.CensusFormId equals cf.CensusFormId
                where (!clientId.HasValue || cf.ClientId == clientId.Value) &&
                      (string.IsNullOrEmpty(monthPeriod) || cf.MonthPeriod == monthPeriod)
                select ca;

            return await query.CountAsync();
        }

        public async Task<int> GetCensusCountAsync(
            int? regionId,
            int? cediId,
            int? zoneId,
            int? route,
            int? month,
            int? year,
            int? userId)
        {
            var userRole = await this._context.UserRoles
                .Where(x => x.UserId == userId)
                .Include(x => x.Role)
                .Select(x => x.Role.RoleName)
                .FirstOrDefaultAsync();

            var query =
                from ca in this._context.CensusAnswer
                join cf in this._context.CensusForm on ca.CensusFormId equals cf.CensusFormId
                join cl in this._context.Clients on cf.ClientId equals cl.ClientId
                join clientAsset in this._context.ClientAssets on ca.ClientAssetId equals clientAsset.ClientAssetId into clientAssetGroup
                from clientAssets in clientAssetGroup.DefaultIfEmpty()
                join a in this._context.Assets on clientAssets.AssetId equals a.AssetId into assetGroup
                from a in assetGroup.DefaultIfEmpty()
                join u in this._context.Users on cl.UserId equals u.UserId into userGroup
                from u in userGroup.DefaultIfEmpty()
                join cedi in this._context.Cedis on u.CediId equals cedi.CediId into cediGroup
                from cedi in cediGroup.DefaultIfEmpty()
                join zone in this._context.Zones on cedi.CediId equals zone.CediId into zoneGroup
                from zone in zoneGroup.DefaultIfEmpty()
                where
                    (regionId == null || (cedi != null && cedi.RegionId == regionId.Value)) &&
                    (cediId == null || (cedi != null && cedi.CediId == cediId.Value)) &&
                    (zoneId == null || (zone != null && zone.ZoneCode == zoneId.Value)) &&
                    (!route.HasValue || cl.Route == route.Value) &&
                    (month == null || (cf.MonthPeriod.Substring(4, 2) == month.Value.ToString("D2"))) &&
                    (!year.HasValue || cf.MonthPeriod.StartsWith(year.Value.ToString()))
                group ca by new { cf.ClientId, ca.ClientAssetId, cf.MonthPeriod } into groupedCensus
                select groupedCensus.Key;

            if (userRole != null && userRole.Equals(RolesConst.Vendedor, StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(c =>
                    this._context.Clients
                        .Any(cl => cl.ClientId == c.ClientId && cl.UserId == userId));
            }

            return await query.CountAsync();
        }

        public async Task<CensusForm?> GetCensusFormAsync(int clientId, string monthPeriod)
        {
            return await this._context.CensusForm
                .FirstOrDefaultAsync(cf => cf.ClientId == clientId && cf.MonthPeriod == monthPeriod);
        }

        public async Task<CensusForm> CreateCensusFormAsync(int clientId, string monthPeriod)
        {
            var form = new CensusForm
            {
                ClientId = clientId,
                MonthPeriod = monthPeriod,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            this._context.CensusForm.Add(form);
            await this._context.SaveChangesAsync();
            return form;
        }

        public async Task<CensusForm?> GetCensusFormByIdAsync(int censusFormId)
        {
            return await this._context.CensusForm
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.CensusFormId == censusFormId);
        }

        public async Task<List<CensusAnswer>> GetCensusAnswersByClientAssetIdAsync(int clientAssetId)
        {
            return await this._context.CensusAnswer
                .AsNoTracking()
                .Where(x => x.ClientAssetId == clientAssetId)
                .ToListAsync();
        }

        public async Task DeleteCensusAnswerAsync(CensusAnswer entity)
        {
            this._context.CensusAnswer.Remove(entity);
            await this._context.SaveChangesAsync();
        }

        public async Task<List<int>> GetCensusClientIdsByPeriodAsync(string period, List<int> clientIds)
        {
            return await this._context.CensusForm
                .Where(cf => cf.MonthPeriod == period && clientIds.Contains(cf.ClientId))
                .Select(cf => cf.ClientId)
                .Distinct()
                .ToListAsync();
        }
    }
}