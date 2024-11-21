using BackEndAje.Api.Domain.Entities;

namespace BackEndAje.Api.Domain.Repositories
{
    public interface IMastersRepository
    {
        Task<List<ReasonRequest>> GetAllReasonRequest();
        
        Task<ReasonRequest?> GetReasonRequestByDescriptionAsync(string reasonDescription);

        Task<List<WithDrawalReason>> GetWithDrawalReasonsByReasonRequestId(int reasonRequestId);
        
        Task<WithDrawalReason?> GetWithDrawalReasonsByDescriptionAsync(string description);

        Task<List<TimeWindow>> GetAllTimeWindows();
        
        Task<TimeWindow?> GetTimeWindowsByTimeRangeAsync(string timeRange);
        
        Task<List<ProductType>> GetAllProductTypes();
        
        Task<List<Logo>> GetAllLogos();
        
        Task<List<ProductSize>> GetAllProductSize();
        
        Task<ProductSize?> GetProductSizeByDescriptionAsync(string description);
        
        Task<List<PaymentMethods>> GetAllPaymentMethods();
        
        Task<PaymentMethods?> GetPaymentMethodById(int paymentMethodId);
        
        Task<List<DocumentType>> GetAllDocumentType();

        Task<DocumentType?> GetDocumentTypeById(int documentTypeId);
        
        Task<List<OrderStatus>> GetAllOrderStatus(int? userId = null);
        
        Task<OrderStatus?> GetOrderStatusByNameAsync(string statusName);
    }
}