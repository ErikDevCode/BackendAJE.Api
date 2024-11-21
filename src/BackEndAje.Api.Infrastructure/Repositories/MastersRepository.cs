namespace BackEndAje.Api.Infrastructure.Repositories
{
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using BackEndAje.Api.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;

    public class MastersRepository : IMastersRepository
    {
        private readonly ApplicationDbContext _context;

        public MastersRepository(ApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<List<ReasonRequest>> GetAllReasonRequest()
        {
            return await this._context.ReasonRequest.Where(x => x.IsActive).ToListAsync();
        }

        public async Task<ReasonRequest?> GetReasonRequestByDescriptionAsync(string reasonDescription)
        {
            return await this._context.ReasonRequest
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.ReasonDescription == reasonDescription);
        }

        public async Task<List<WithDrawalReason>> GetWithDrawalReasonsByReasonRequestId(int reasonRequestId)
        {
            return await this._context.WithDrawalReason.AsNoTracking()
                .Where(wr => wr.ReasonRequestId == reasonRequestId)
                .ToListAsync();
        }

        public async Task<WithDrawalReason?> GetWithDrawalReasonsByDescriptionAsync(string description)
        {
            return await this._context.WithDrawalReason
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.WithDrawalReasonDescription == description);
        }

        public async Task<List<TimeWindow>> GetAllTimeWindows()
        {
            return await this._context.TimeWindows.Where(x => x.IsActive).ToListAsync();
        }

        public async Task<TimeWindow?> GetTimeWindowsByTimeRangeAsync(string timeRange)
        {
            return await this._context.TimeWindows
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.TimeRange == timeRange);
        }

        public async Task<List<ProductType>> GetAllProductTypes()
        {
            return await this._context.ProductTypes.Where(x => x.IsActive).ToListAsync();
        }

        public async Task<List<Logo>> GetAllLogos()
        {
            return await this._context.Logos.Where(x => x.IsActive).ToListAsync();
        }

        public async Task<List<ProductSize>> GetAllProductSize()
        {
            return await this._context.ProductSize.Where(x => x.IsActive).ToListAsync();
        }

        public async Task<ProductSize?> GetProductSizeByDescriptionAsync(string description)
        {
            return await this._context.ProductSize
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.ProductSizeDescription == description);
        }

        public async Task<List<PaymentMethods>> GetAllPaymentMethods()
        {
            return await this._context.PaymentMethods.ToListAsync();
        }

        public async Task<PaymentMethods?> GetPaymentMethodById(int paymentMethodId)
        {
            return await this._context.PaymentMethods.FirstOrDefaultAsync(x => x.PaymentMethodId == paymentMethodId);
        }

        public async Task<List<DocumentType>> GetAllDocumentType()
        {
            return await this._context.DocumentType.ToListAsync();
        }

        public async Task<DocumentType?> GetDocumentTypeById(int documentTypeId)
        {
            return await this._context.DocumentType.FirstOrDefaultAsync(x => x.DocumentTypeId == documentTypeId);
        }

        public async Task<List<OrderStatus>> GetAllOrderStatus(int? userId)
        {
            if (userId == null)
            {
                return await this._context.OrderStatus
                    .Where(os => os.IsActive)
                    .ToListAsync();
            }
            else
            {
                var userRoleId = await this._context.UserRoles
                    .Where(ur => ur.UserId == userId.Value)
                    .Select(ur => ur.RoleId)
                    .FirstOrDefaultAsync();

                if (userRoleId == 1)
                {
                    return await this._context.OrderStatus
                        .Where(os => os.IsActive)
                        .ToListAsync();
                }

                return await this._context.OrderStatus
                    .Where(os => os.IsActive && this._context.OrderStatusRoles
                        .Any(osr => osr.OrderStatusId == os.OrderStatusId && osr.RoleId == userRoleId))
                    .ToListAsync();
            }
        }

        public async Task<OrderStatus?> GetOrderStatusByNameAsync(string statusName)
        {
            return await this._context.OrderStatus
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.StatusName == statusName);
        }
    }
}

