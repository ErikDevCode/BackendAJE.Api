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
                .Include(o => o.Supervisor)
                .Include(o => o.Sucursal)
                .Include(o => o.ReasonRequest)
                .Include(o => o.TimeWindow)
                .Include(o => o.WithDrawalReason)
                .Include(o => o.ProductType)
                .Include(o => o.Logo)
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
                .SingleOrDefaultAsync(o => o.OrderRequestId == id);

            return orderRequest;
        }

        public async Task<OrderRequestDocument> GetOrderRequestDocumentById(int id)
        {
            return (await this._context.OrderRequestDocuments.AsNoTracking().FirstOrDefaultAsync(r => r.DocumentId == id))!;
        }

        public async Task<List<OrderRequest>> GetAllOrderRequestAsync(int pageNumber, int pageSize, int? clientCode, int? orderStatusId, int? reasonRequestId, DateTime? startDate, DateTime? endDate, int? supervisorId = null, int? vendedorId = null)
        {
            var query = this._context.OrderRequests
                .Include(o => o.Supervisor)
                .Include(o => o.Sucursal)
                .Include(o => o.ReasonRequest)
                .Include(o => o.TimeWindow)
                .Include(o => o.WithDrawalReason)
                .Include(o => o.ProductType)
                .Include(o => o.Logo)
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

            if (startDate.HasValue)
            {
                query = query.Where(o => o.CreatedAt >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(o => o.CreatedAt <= endDate.Value);
            }

            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetTotalOrderRequestCountAsync(int? clientCode, int? orderStatusId, int? reasonRequestId, DateTime? startDate, DateTime? endDate, int? supervisorId = null, int? vendedorId = null)
        {
            var query = this._context.OrderRequests.AsQueryable();

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

        public async Task UpdateStatusOrderRequestDocumentAsync(OrderRequestDocument orderRequestDocument)
        {
            this._context.Entry(orderRequestDocument).State = EntityState.Detached;
            this._context.OrderRequestDocuments.Update(orderRequestDocument);
            await this._context.SaveChangesAsync();
        }

        public async Task AddOrderRequestStatusHistoryAsync(OrderRequestStatusHistory orderRequestStatusHistory)
        {
            this._context.OrderRequestStatusHistory.Add(orderRequestStatusHistory);
            await this._context.SaveChangesAsync();
        }

        public async Task UpdateStatusOrderRequestAsync(int orderRequestId, int newStatusId, int createdBy)
        {
            var orderRequest = new OrderRequest { OrderRequestId = orderRequestId, OrderStatusId = newStatusId, UpdatedBy = createdBy };

            this._context.OrderRequests.Attach(orderRequest);
            this._context.Entry(orderRequest).Property(o => o.OrderStatusId).IsModified = true;

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
    }
}
