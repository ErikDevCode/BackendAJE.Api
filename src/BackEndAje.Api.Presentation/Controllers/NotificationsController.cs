namespace BackEndAje.Api.Presentation.Controllers
{
    using BackEndAje.Api.Application.Notification.Commands.CreateNotification;
    using BackEndAje.Api.Application.Notification.Commands.NotificationMarkAsRead;
    using BackEndAje.Api.Application.Notification.Commands.SendNotification;
    using BackEndAje.Api.Application.Notification.Queries.GetNotificationByUserId;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]

    public class NotificationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NotificationsController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpPost]
        [Route("notify")]
        public async Task<IActionResult> Notify([FromBody] SendNotificationCommand command)
        {
            await this._mediator.Send(command);
            return this.Ok("Notification sent.");
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateNotification([FromBody] CreateNotificationCommand command)
        {
            await this._mediator.Send(command);
            return this.Ok();
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetNotificationsByUserId(int userId)
        {
            var query = new GetNotificationByUserIdQuery(userId);
            var notifications = await this._mediator.Send(query);
            return this.Ok(notifications);
        }

        [HttpPatch("{notificationId}/mark-as-read")]
        public async Task<IActionResult> MarkAsRead(int notificationId)
        {
            var command = new NotificationMarkAsReadCommand { NotificationId = notificationId };
            await this._mediator.Send(command);
            return this.Ok();
        }
    }
}

