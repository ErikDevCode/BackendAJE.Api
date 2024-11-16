namespace BackEndAje.Api.Infrastructure.Repositories
{
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using BackEndAje.Api.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;

    public class NotificationRepository : INotificationRepository
    {
        private readonly ApplicationDbContext _context;

        public NotificationRepository(ApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task AddNotificationAsync(Notification notification)
        {
            this._context.Notifications.Add(notification);
            await this._context.SaveChangesAsync();
        }

        public async Task<List<Notification?>> GetAllNotificationAsync(int userId)
        {
            return (await this._context.Notifications.AsNoTracking().Where(r => r.UserId == userId && !r.IsRead).ToListAsync())!;
        }

        public async Task MarkAsReadAsync(int notificationId)
        {
            var notification = await this._context.Notifications.FindAsync(notificationId);

            if (notification != null)
            {
                notification.IsRead = true;
                await this._context.SaveChangesAsync();
            }
        }
    }
}