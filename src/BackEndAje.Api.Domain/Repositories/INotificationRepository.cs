using BackEndAje.Api.Domain.Entities;

namespace BackEndAje.Api.Domain.Repositories
{
    public interface INotificationRepository
    {
        Task AddNotificationAsync(Notification notification);
        
        Task<List<Notification?>> GetAllNotificationAsync(int userId);

        Task MarkAsReadAsync(int notificationId);
    }
}