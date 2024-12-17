namespace BackEndAje.Api.Infrastructure.Repositories
{
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using BackEndAje.Api.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;

    public class OrderRequestRepository : IOrderRequestRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRequestRepository(ApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task AddOrderRequestAsync(OrderRequest orderRequest)
        {
            this._context.OrderRequests.Add(orderRequest);
            await this._context.SaveChangesAsync();
        }

        public async Task AddOrderRequestDocumentAsync(OrderRequestDocument orderRequestDocument)
        {
            this._context.OrderRequestDocuments.Add(orderRequestDocument);
            await this._context.SaveChangesAsync();
        }

        public async Task<OrderRequest?> GetOrderRequestById(int id)
        {
            var orderRequest = await this._context.OrderRequests
                .AsNoTracking()
                .Include(o => o.Supervisor)
                .Include(o => o.Sucursal)
                .Include(o => o.ReasonRequest)
                .Include(o => o.TimeWindow)
                .Include(o => o.WithDrawalReason)
                .Include(o => o.ProductSize)
                .Include(o => o.Client)
                .ThenInclude(c => c.Seller)
                .ThenInclude(s => s.Cedi)
                .Include(o => o.Client)
                .ThenInclude(c => c.Seller)
                .ThenInclude(s => s.Zone)
                .Include(o => o.Client)
                .ThenInclude(d => d.District)
                .Include(o => o.Client)
                .ThenInclude(t => t.DocumentType)
                .Include(o => o.OrderRequestDocuments)
                .Include(oa => oa.OrderRequestAssets)
                .ThenInclude(a => a.Asset)
                .SingleOrDefaultAsync(o => o.OrderRequestId == id);

            if (orderRequest != null)
            {
                orderRequest.OrderRequestAssets = orderRequest.OrderRequestAssets
                    .Where(a => a.IsActive is true or null)
                    .ToList();
            }

            return orderRequest;
        }

        public async Task<List<OrderRequestDocument>> GetOrderRequestDocumentByOrderRequestId(int id)
        {
            return await this._context.OrderRequestDocuments.Where(x => x.OrderRequestId == id).ToListAsync();
        }

        public async Task<OrderRequestDocument> GetOrderRequestDocumentById(int id)
        {
            return (await this._context.OrderRequestDocuments.AsNoTracking().FirstOrDefaultAsync(x => x.DocumentId == id)) !;
        }

        public async Task DeleteOrderRequestDocumentAsync(OrderRequestDocument orderRequestDocument)
        {
            this._context.OrderRequestDocuments.Remove(orderRequestDocument);
            await this._context.SaveChangesAsync();
        }

        public async Task<List<OrderRequest>> GetAllOrderRequestAsync(int? pageNumber, int? pageSize, int? clientCode, int? orderStatusId, int? reasonRequestId, int? cediId, int? regionId, DateTime? startDate, DateTime? endDate, int? supervisorId = null, int? vendedorId = null)
        {
            var query = this._context.OrderRequests
                .Where(x => x.IsActive == true)
                .Include(o => o.Supervisor)
                .Include(o => o.Sucursal)
                    .ThenInclude(s => s.Region)
                .Include(o => o.ReasonRequest)
                .Include(o => o.TimeWindow)
                .Include(o => o.WithDrawalReason)
                .Include(o => o.ProductSize)
                .Include(o => o.Client)
                    .ThenInclude(c => c.Seller)
                    .ThenInclude(s => s.Cedi)
                .Include(o => o.Client)
                    .ThenInclude(c => c.Seller)
                    .ThenInclude(s => s.Zone)
                .Include(o => o.Client)
                    .ThenInclude(d => d.District)
                .Include(o => o.Client)
                    .ThenInclude(t => t.DocumentType)
                .Include(o => o.OrderRequestDocuments)
                .Include(o => o.OrderStatus)
                .Include(x => x.RelocationRequests.Where(r => r.IsActive == true))
                    .ThenInclude(x => x.OrderStatus)
                .Include(x => x.RelocationRequests.Where(r => r.IsActive == true))
                    .ThenInclude(x => x.Relocation)
                .Include(x => x.RelocationRequests.Where(r => r.IsActive == true))
                    .ThenInclude(x => x.ReasonRequest)
                .AsQueryable();

            if (supervisorId.HasValue)
            {
                query = query.Where(o => o.Supervisor.UserId == supervisorId.Value);
            }

            if (vendedorId.HasValue)
            {
                query = query.Where(o => o.Client.Seller!.UserId == vendedorId.Value);
            }

            if (clientCode.HasValue)
            {
                query = query.Where(o => o.Client.ClientCode == clientCode.Value);
            }

            if (orderStatusId.HasValue)
            {
                query = query.Where(o => o.OrderStatusId == orderStatusId.Value);
            }

            if (reasonRequestId.HasValue)
            {
                query = query.Where(o => o.ReasonRequestId == reasonRequestId.Value);
            }

            if (cediId.HasValue)
            {
                query = query.Where(or => or.CediId == cediId.Value);
            }

            if (regionId.HasValue)
            {
                query = query.Where(o => o.Sucursal.RegionId == regionId!.Value);
            }

            if (startDate.HasValue)
            {
                query = query.Where(o => o.CreatedAt >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(o => o.CreatedAt <= endDate.Value);
            }

            if (pageNumber.HasValue && pageSize.HasValue && pageNumber > 0 && pageSize > 0)
            {
                query = query
                    .OrderBy(ca => ca.CreatedAt)
                    .Skip((pageNumber.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<int> GetTotalOrderRequestCountAsync(int? clientCode, int? orderStatusId, int? reasonRequestId, int? cediId, int? regionId, DateTime? startDate, DateTime? endDate, int? supervisorId = null, int? vendedorId = null)
        {
            var query = this._context.OrderRequests.Where(x => x.IsActive == true).AsQueryable();

            if (supervisorId.HasValue)
            {
                query = query.Where(o => o.Supervisor.UserId == supervisorId.Value);
            }

            if (vendedorId.HasValue)
            {
                query = query.Where(o => o.Client.Seller!.UserId == vendedorId.Value);
            }

            if (clientCode.HasValue)
            {
                query = query.Where(o => o.Client.ClientCode == clientCode.Value);
            }

            if (orderStatusId.HasValue)
            {
                query = query.Where(o => o.OrderStatusId == orderStatusId.Value);
            }

            if (reasonRequestId.HasValue)
            {
                query = query.Where(o => o.ReasonRequestId == reasonRequestId.Value);
            }

            if (cediId.HasValue)
            {
                query = query.Where(o => o.CediId == cediId.Value);
            }

            if (regionId.HasValue)
            {
                query = query.Where(o => o.Sucursal.RegionId == regionId!.Value);
            }


            if (startDate.HasValue)
            {
                query = query.Where(o => o.CreatedAt >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(o => o.CreatedAt <= endDate.Value);
            }

            return await query.CountAsync();
        }

        public async Task DeleteOrderRequestAsync(OrderRequest orderRequest)
        {
            this._context.Entry(orderRequest).State = EntityState.Detached;
            this._context.OrderRequests.Update(orderRequest);
            await this._context.SaveChangesAsync();
        }

        public async Task AddOrderRequestStatusHistoryAsync(OrderRequestStatusHistory orderRequestStatusHistory)
        {
            this._context.OrderRequestStatusHistory.Add(orderRequestStatusHistory);
            await this._context.SaveChangesAsync();
        }

        public async Task UpdateStatusOrderRequestAsync(int orderRequestId, int newStatusId, int createdBy)
        {
            this._context.ChangeTracker.Clear();
            var existingOrderRequest = await this._context.OrderRequests
                .AsNoTracking()
                .SingleOrDefaultAsync(o => o.OrderRequestId == orderRequestId);

            existingOrderRequest!.OrderStatusId = newStatusId;
            existingOrderRequest.UpdatedBy = createdBy;
            existingOrderRequest.UpdatedAt = DateTime.Now;

            this._context.Entry(existingOrderRequest).State = EntityState.Modified;

            await this._context.SaveChangesAsync();
        }

        public async Task<List<OrderRequestStatusHistory>> GetOrderRequestStatusHistoryByOrderRequestId(int orderRequestId)
        {
            return await this._context.OrderRequestStatusHistory
                .Include(x => x.OrderRequest)
                .Include(x => x.OrderStatus)
                .Include(x => x.CreatedByUser)
                .Where(o => o.OrderRequestId == orderRequestId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        public async Task UpdateOrderRequestDocumentAsync(OrderRequestDocument orderRequestDocument)
        {
            this._context.Entry(orderRequestDocument).State = EntityState.Detached;
            this._context.OrderRequestDocuments.Update(orderRequestDocument);
            await this._context.SaveChangesAsync();
        }

        public async Task<int> GetTotalOrderRequestStatusCount(
            int? statusId = null,
            int? supervisorId = null,
            int? vendedorId = null,
            int? regionId = null,
            int? zoneId = null,
            int? route = null,
            int? month = null,
            int? year = null)
        {
            var query = this._context.OrderRequests
                .Include(c => c.Client)
                    .ThenInclude(c => c.Seller)
                        .ThenInclude(s => s!.Cedi)
                            .ThenInclude(c => c.Region)
                .Include(o => o.Client)
                    .ThenInclude(c => c.Seller)
                        .ThenInclude(s => s!.Zone)
                .Include(d => d.Supervisor).AsQueryable();

            if (statusId.HasValue)
            {
                query = query.Where(x => x.OrderStatusId == statusId.Value);
            }

            if (supervisorId.HasValue)
            {
                query = query.Where(x => x.Supervisor.UserId == supervisorId.Value);
            }

            if (vendedorId.HasValue)
            {
                query = query.Where(x => x.Client.Seller!.UserId == vendedorId.Value);
            }

            if (regionId.HasValue)
            {
                query = query.Where(x => x.Client.Seller!.Cedi!.Region!.RegionId == regionId.Value);
            }

            if (zoneId.HasValue)
            {
                query = query.Where(x => x.Client.Seller!.Zone!.ZoneId == zoneId.Value);
            }

            if (route.HasValue)
            {
                query = query.Where(x => x.Client.Seller!.Route == route.Value);
            }
            if (month.HasValue && year.HasValue)
            {
                query = query.Where(x => x.CreatedAt.Month == month.Value &&
                                         x.CreatedAt.Year == year.Value);
            }
            else if (year.HasValue)
            {
                query = query.Where(x => x.CreatedAt.Year == year.Value);
            }

            return await query.CountAsync();
        }

        public async Task<int> GetTotalOrderRequestReasonCount(
            int? reasonRequestId = null,
            int? supervisorId = null,
            int? vendedorId = null,
            int? regionId = null,
            int? zoneId = null,
            int? route = null,
            int? month = null,
            int? year = null)
        {
            var query = this._context.OrderRequests
                .Include(c => c.Client)
                .ThenInclude(c => c.Seller)
                .ThenInclude(s => s!.Cedi)
                .ThenInclude(c => c.Region)
                .Include(o => o.Client)
                .ThenInclude(c => c.Seller)
                .ThenInclude(s => s!.Zone)
                .Include(d => d.Supervisor).AsQueryable();

            if (reasonRequestId.HasValue)
            {
                query = query.Where(x => x.ReasonRequestId == reasonRequestId.Value);
            }

            if (supervisorId.HasValue)
            {
                query = query.Where(x => x.Supervisor.UserId == supervisorId.Value);
            }

            if (vendedorId.HasValue)
            {
                query = query.Where(x => x.Client.Seller!.UserId == vendedorId.Value);
            }

            if (regionId.HasValue)
            {
                query = query.Where(x => x.Client.Seller!.Cedi!.Region!.RegionId == regionId.Value);
            }

            if (zoneId.HasValue)
            {
                query = query.Where(x => x.Client.Seller!.Zone!.ZoneId == zoneId.Value);
            }

            if (route.HasValue)
            {
                query = query.Where(x => x.Client.Seller!.Route == route.Value);
            }

            if (month.HasValue && year.HasValue)
            {
                query = query.Where(x => x.CreatedAt.Month == month.Value &&
                                         x.CreatedAt.Year == year.Value);
            }
            else if (year.HasValue)
            {
                query = query.Where(x => x.CreatedAt.Year == year.Value);
            }

            return await query.CountAsync();
        }

        public async Task<int> GetTotalAssetFromOrderRequestStatusAttendedCount(int? supervisorId = null, int? vendedorId = null,
            int? regionId = null, int? zoneId = null, int? route = null, int? month = null, int? year = null)
        {
            var query = this._context.OrderRequestAssets
                .Include(oa => oa.OrderRequest)
                .ThenInclude(or => or.Client)
                .ThenInclude(c => c.Seller)
                .ThenInclude(s => s!.Cedi)
                .ThenInclude(c => c.Region)
                .Include(oa => oa.OrderRequest)
                .ThenInclude(or => or.Client)
                .ThenInclude(c => c.Seller)
                .ThenInclude(s => s!.Zone)
                .Include(oa => oa.OrderRequest)
                .ThenInclude(or => or.Supervisor)
                .Where(oa => oa.OrderRequest.OrderStatusId == 5)
                .AsQueryable();

            if (supervisorId.HasValue)
            {
                query = query.Where(oa => oa.OrderRequest.Supervisor.UserId == supervisorId.Value);
            }

            if (vendedorId.HasValue)
            {
                query = query.Where(oa => oa.OrderRequest.Client.Seller!.UserId == vendedorId.Value);
            }

            if (regionId.HasValue)
            {
                query = query.Where(oa => oa.OrderRequest.Client.Seller!.Cedi!.Region!.RegionId == regionId.Value);
            }

            if (zoneId.HasValue)
            {
                query = query.Where(oa => oa.OrderRequest.Client.Seller!.Zone!.ZoneId == zoneId.Value);
            }

            if (route.HasValue)
            {
                query = query.Where(oa => oa.OrderRequest.Client.Seller!.Route == route.Value);
            }

            if (month.HasValue && year.HasValue)
            {
                query = query.Where(oa => oa.OrderRequest.CreatedAt.Month == month.Value &&
                                          oa.OrderRequest.CreatedAt.Year == year.Value);
            }
            else if (year.HasValue)
            {
                query = query.Where(oa => oa.OrderRequest.CreatedAt.Year == year.Value);
            }

            return await query.CountAsync();
        }

        public async Task<int> AssignAssetToOrder(int orderRequestId, int assetId, int assignedBy)
        {
            var orderRequest = this._context.OrderRequests.AsNoTracking()
                .FirstOrDefault(x => x.OrderRequestId == orderRequestId);

            var orderRequestAsset = new OrderRequestAssets
            {
                OrderRequestId = orderRequestId,
                AssetId = assetId,
                IsActive = true,
                CreatedBy = assignedBy,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                UpdatedBy = assignedBy,
            };

            var requestReason = this._context.OrderRequestAssets.AsNoTracking()
                .FirstOrDefault(x => x.OrderRequestId == orderRequestId);
            if (orderRequest!.ReasonRequestId == 3 && requestReason != null)
            {
                orderRequestAsset.IsActive = null;
            }

            await this._context.OrderRequestAssets.AddAsync(orderRequestAsset);
            await this._context.SaveChangesAsync();
            return orderRequestAsset.OrderRequestAssetId;
        }

        public async Task AddOrderRequestAssetTrace(OrderRequestAssetsTrace orderRequestAssetsTrace)
        {
            this._context.OrderRequestAssetsTrace.Add(orderRequestAssetsTrace);
            await this._context.SaveChangesAsync();
        }

        public async Task<OrderRequestAssets> GetOrderRequestAssetsById(int orderRequestAssetId)
        {
            return (await this._context.OrderRequestAssets.AsNoTracking().FirstOrDefaultAsync(
                x => x.OrderRequestAssetId == orderRequestAssetId))!;
        }

        public async Task UpdateAssetToOrderRequest(OrderRequestAssets orderRequestAssets)
        {
            this._context.Entry(orderRequestAssets).State = EntityState.Detached;
            this._context.OrderRequestAssets.Update(orderRequestAssets);
            await this._context.SaveChangesAsync();
        }

        public async Task<List<OrderRequestAssetsTrace>> GetOrderRequestAssetsTraceByOrderRequestId(int orderRequestId)
        {
            return await this._context.OrderRequestAssetsTrace
                .Include(x => x.Asset)
                .Include(x => x.OrderRequest)
                .Include(x => x.OrderRequestAssets)
                .Include(x => x.User)
                .Where(o => o.OrderRequestId == orderRequestId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        public async Task BulkInsertOrderRequestsAsync(IEnumerable<OrderRequest> orderRequests)
        {
            await this._context.OrderRequests.AddRangeAsync(orderRequests);
            await this._context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int reasonRequestId, int clientId, DateTime negotiatedDate)
        {
            return await this._context.OrderRequests
                .AnyAsync(o =>
                    o.ReasonRequestId == reasonRequestId &&
                    o.ClientId == clientId &&
                    o.NegotiatedDate.Date == negotiatedDate.Date);
        }

        public async Task<List<OrderRequest>> GetAllAsync(int? clientCode, int? orderStatusId, int? reasonRequestId, int? cediId, int? regionId, DateTime? startDate, DateTime? endDate, int? supervisorId = null, int? vendedorId = null)
        {
            var query = this._context.OrderRequests
                .Where(x => x.IsActive == true)
                .Include(o => o.Supervisor)
                .Include(o => o.Sucursal)
                    .ThenInclude(s => s.Region)
                .Include(o => o.ReasonRequest)
                .Include(o => o.TimeWindow)
                .Include(o => o.WithDrawalReason)
                .Include(o => o.ProductSize)
                .Include(o => o.Client)
                    .ThenInclude(c => c.Seller)
                    .ThenInclude(s => s.Cedi)
                .Include(o => o.Client)
                    .ThenInclude(c => c.Seller)
                    .ThenInclude(s => s.Zone)
                .Include(o => o.Client)
                    .ThenInclude(d => d.District)
                .Include(o => o.Client)
                    .ThenInclude(t => t.DocumentType)
                .Include(o => o.OrderRequestDocuments)
                .Include(o => o.OrderStatus)
                .Include(x => x.RelocationRequests.Where(r => r.IsActive == true))
                    .ThenInclude(x => x.OrderStatus)
                .Include(x => x.RelocationRequests.Where(r => r.IsActive == true))
                    .ThenInclude(x => x.Relocation)
                .Include(x => x.RelocationRequests.Where(r => r.IsActive == true))
                    .ThenInclude(x => x.ReasonRequest)
                .AsQueryable();

            if (supervisorId.HasValue)
            {
                query = query.Where(o => o.Supervisor.UserId == supervisorId.Value);
            }

            if (vendedorId.HasValue)
            {
                query = query.Where(o => o.Client.Seller!.UserId == vendedorId.Value);
            }

            if (clientCode.HasValue)
            {
                query = query.Where(o => o.Client.ClientCode == clientCode.Value);
            }

            if (orderStatusId.HasValue)
            {
                query = query.Where(o => o.OrderStatusId == orderStatusId.Value);
            }

            if (reasonRequestId.HasValue)
            {
                query = query.Where(o => o.ReasonRequestId == reasonRequestId.Value);
            }

            if (cediId.HasValue)
            {
                query = query.Where(or => or.CediId == cediId.Value);
            }

            if (regionId.HasValue)
            {
                query = query.Where(o => o.Sucursal.RegionId == regionId!.Value);
            }

            if (startDate.HasValue)
            {
                query = query.Where(o => o.CreatedAt >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(o => o.CreatedAt <= endDate.Value);
            }

            return await query.ToListAsync();
        }

        public async Task AddRelocation(Relocation relocation)
        {
            this._context.Relocation.Add(relocation);
            await this._context.SaveChangesAsync();
        }

        public async Task AddRelocationRequests(RelocationRequest relocationRequest)
        {
            this._context.RelocationRequests.Add(relocationRequest);
            await this._context.SaveChangesAsync();
        }

        public async Task<RelocationRequest> GetRelocationRequestByOrderRequestId(int orderRequestId)
        {
            return (await this._context.RelocationRequests.AsNoTracking().FirstOrDefaultAsync(
                x => x.OrderRequestId == orderRequestId))!;
        }

        public async Task UpdateRelocationRequest(RelocationRequest relocationRequest)
        {
            this._context.Entry(relocationRequest).State = EntityState.Detached;
            this._context.RelocationRequests.Update(relocationRequest);
            await this._context.SaveChangesAsync();
        }

        public async Task<List<RelocationRequest>> GetListRelocationRequestByOrderRequestId(int orderRequestId)
        {
            var relocationId = await this._context.RelocationRequests
                .Where(rr => rr.OrderRequestId == orderRequestId)
                .Select(rr => rr.RelocationId)
                .FirstOrDefaultAsync();

            if (relocationId == 0)
            {
                return new List<RelocationRequest>();
            }

            return await this._context.RelocationRequests
                .Where(rr => rr.RelocationId == relocationId)
                .ToListAsync();
        }

        public async Task DeleteRelocationAsync(Relocation relocation)
        {
            this._context.Entry(relocation).State = EntityState.Detached;
            this._context.Relocation.Update(relocation);
            await this._context.SaveChangesAsync();
        }

        public async Task DeleteRelocationRequestAsync(RelocationRequest relocationRequest)
        {
            this._context.Entry(relocationRequest).State = EntityState.Detached;
            this._context.RelocationRequests.Update(relocationRequest);
            await this._context.SaveChangesAsync();
        }

        public async Task<Relocation> GetRelocationById(int relocationId)
        {
            return (await this._context.Relocation.AsNoTracking().FirstOrDefaultAsync(
                x => x.RelocationId == relocationId)) !;
        }

        public async Task<List<RelocationRequest>> GetRelocationRequestByRelocationId(int relocationId)
        {
            return await this._context.RelocationRequests.Where(
                x => x.RelocationId == relocationId).ToListAsync();
        }
    }
}
