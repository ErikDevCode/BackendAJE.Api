using BackEndAje.Api.Domain.Entities;

namespace BackEndAje.Api.Domain.Repositories
{
    public interface IOrderRequestRepository
    {
        Task AddOrderRequestAsync(OrderRequest orderRequest);
        
        Task AddOrderRequestDocumentAsync(OrderRequestDocument orderRequestDocument);
        
        Task<OrderRequest?> GetOrderRequestById(int id);

        Task<List<OrderRequestDocument>> GetOrderRequestDocumentByOrderRequestId(int id);
        
        Task<OrderRequestDocument> GetOrderRequestDocumentById(int id);
        
        Task DeleteOrderRequestDocumentAsync(OrderRequestDocument orderRequestDocument);

        Task<List<OrderRequest>> GetAllOrderRequestAsync(int pageNumber, int pageSize, int? clientCode, int? orderStatusId, int? reasonRequestId, DateTime? startDate, DateTime? endDate, int? supervisorId = null, int? vendedorId = null);

        Task<int> GetTotalOrderRequestCountAsync(int? clientCode, int? orderStatusId, int? reasonRequestId, DateTime? startDate, DateTime? endDate, int? supervisorId = null, int? vendedorId = null);
        
        Task UpdateStatusOrderRequestDocumentAsync(OrderRequestDocument orderRequestDocument);
        
        Task AddOrderRequestStatusHistoryAsync(OrderRequestStatusHistory orderRequestStatusHistory);

        Task UpdateStatusOrderRequestAsync(int orderRequestId, int newStatusId, int createdBy);
        
        Task<List<OrderRequestStatusHistory>> GetOrderRequestStatusHistoryByOrderRequestId(int orderRequestId);
        
        Task UpdateOrderRequestDocumentAsync(OrderRequestDocument orderRequestDocument);

        Task<int> GetTotalOrderRequestStatusCount(
            int? statusId = null,
            int? supervisorId = null,
            int? vendedorId = null,
            int? regionId = null,
            int? zoneId = null,
            int? route = null,
            int? month = null,
            int? year = null);

        Task<int> GetTotalOrderRequestReasonCount(
            int? reasonRequestId = null,
            int? supervisorId = null,
            int? vendedorId = null,
            int? regionId = null,
            int? zoneId = null,
            int? route = null,
            int? month = null,
            int? year = null);
    }
}
