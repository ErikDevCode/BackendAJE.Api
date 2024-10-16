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

        public async Task<List<OrderRequest>> GetAllOrderRequestAsync(int pageNumber, int pageSize)
        {
            return await this._context.OrderRequests
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
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
        }

        public async Task<int> GetTotalOrderRequestCountAsync()
        {
            return await this._context.Roles.CountAsync();
        }
    }
}
