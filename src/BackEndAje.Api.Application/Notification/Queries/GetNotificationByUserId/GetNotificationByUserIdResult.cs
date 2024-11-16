namespace BackEndAje.Api.Application.Notification.Queries.GetNotificationByUserId
{
    public class GetNotificationByUserIdResult
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Message { get; set; }

        public bool IsRead { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}