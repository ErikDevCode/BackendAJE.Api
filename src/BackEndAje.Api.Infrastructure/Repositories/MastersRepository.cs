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

        public async Task<List<WithDrawalReason>> GetWithDrawalReasonsByReasonRequestId(int reasonRequestId)
        {
            return await this._context.WithDrawalReason.AsNoTracking()
                .Where(wr => wr.ReasonRequestId == reasonRequestId)
                .ToListAsync();
        }

        public async Task<List<TimeWindow>> GetAllTimeWindows()
        {
            return await this._context.TimeWindows.Where(x => x.IsActive).ToListAsync();
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

        public async Task<List<OrderStatus>> GetAllOrderStatus()
        {
            return await this._context.OrderStatus.Where(x => x.IsActive).ToListAsync();
        }
    }
}

