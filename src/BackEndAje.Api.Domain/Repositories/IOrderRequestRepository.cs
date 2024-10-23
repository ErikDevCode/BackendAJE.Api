using BackEndAje.Api.Domain.Entities;

namespace BackEndAje.Api.Domain.Repositories
{
    public interface IOrderRequestRepository
    {
        Task AddOrderRequestAsync(OrderRequest orderRequest);
        
        Task AddOrderRequestDocumentAsync(OrderRequestDocument orderRequestDocument);
        
        Task<OrderRequest?> GetOrderRequestById(int id);

        Task<OrderRequestDocument> GetOrderRequestDocumentById(int id);

        Task<List<OrderRequest>> GetAllOrderRequestAsync(int pageNumber, int pageSize, int? clientCode, int? orderStatusId, int? reasonRequestId, DateTime? startDate, DateTime? endDate);

        Task<int> GetTotalOrderRequestCountAsync(int? clientCode, int? orderStatusId, int? reasonRequestId, DateTime? startDate, DateTime? endDate);
        
        Task UpdateStatusOrderRequestDocumentAsync(OrderRequestDocument orderRequestDocument);
        
        Task AddOrderRequestStatusHistoryAsync(OrderRequestStatusHistory orderRequestStatusHistory);

        Task UpdateStatusOrderRequestAsync(int orderRequestId, int newStatusId, int createdBy);
        
        Task<List<OrderRequestStatusHistory>> GetOrderRequestStatusHistoryByOrderRequestId(int orderRequestId);
    }
}
