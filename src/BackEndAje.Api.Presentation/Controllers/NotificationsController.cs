namespace BackEndAje.Api.Presentation.Controllers
{
    using BackEndAje.Api.Application.Notification.Commands.SendNotification;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
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
    }
}

