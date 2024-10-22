using BackEndAje.Api.Domain.Entities;

namespace BackEndAje.Api.Domain.Repositories
{
    public interface IOrderRequestRepository
    {
        Task AddOrderRequestAsync(OrderRequest orderRequest);
        
        Task AddOrderRequestDocumentAsync(OrderRequestDocument orderRequestDocument);
        
        Task<OrderRequest?> GetOrderRequestById(int id);

        Task<OrderRequestDocument> GetOrderRequestDocumentById(int id);

        Task<List<OrderRequest>> GetAllOrderRequestAsync(int pageNumber, int pageSize);

        Task<int> GetTotalOrderRequestCountAsync();
        
        Task UpdateStatusOrderRequestDocumentAsync(OrderRequestDocument orderRequestDocument);
        
        Task AddOrderRequestStatusHistoryAsync(OrderRequestStatusHistory orderRequestStatusHistory);
    }
}
