namespace BackEndAje.Api.Presentation.Controllers
{
    using System.Net;
    using BackEndAje.Api.Application.Dtos;
    using BackEndAje.Api.Application.Users.Commands.CreateUser;
    using BackEndAje.Api.Application.Users.Commands.UpdateUser;
    using BackEndAje.Api.Application.Users.Queries.GetUserByRouteOrEmail;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    /// <inheritdoc />
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(typeof(CreateUserResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IDictionary<string, string>), (int)HttpStatusCode.BadRequest)]
        [Route("create")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
        {
            var userId = this.GetUserId();
            command.User.CreatedBy = userId;
            command.User.UpdatedBy = userId;
            var result = await this._mediator.Send(command);
            return this.Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(CreateUserResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IDictionary<string, string>), (int)HttpStatusCode.BadRequest)]
        [Route("{routeOrEmail}")]
        public async Task<IActionResult> GetUserByRouteOrEmail(string routeOrEmail)
        {
            var query = new GetUserByRouteOrEmailQuery(routeOrEmail);
            var result = await this._mediator.Send(query);
            return this.Ok(new Response { Result = result });
        }

        [HttpPut]
        [ProducesResponseType(typeof(CreateUserResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IDictionary<string, string>), (int)HttpStatusCode.BadRequest)]
        [Route("update")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserCommand command)
        {
            var userId = this.GetUserId();
            command.User.UpdatedBy = userId;
            var result = await this._mediator.Send(command);
            return this.Ok(result);
        }

        private int GetUserId()
        {
            var userIdClaim = this.User.FindFirst("UserId") ?? this.User.FindFirst("sub");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                throw new UnauthorizedAccessException("User ID not found or invalid in token.");
            }

            return userId;
        }
    }
}
