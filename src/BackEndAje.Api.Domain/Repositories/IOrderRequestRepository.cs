using BackEndAje.Api.Domain.Entities;

namespace BackEndAje.Api.Domain.Repositories
{
    public interface IOrderRequestRepository
    {
        Task AddOrderRequestAsync(OrderRequest orderRequest);
        
        Task AddOrderRequestDocumentAsync(OrderRequestDocument orderRequestDocument);
    }
}
