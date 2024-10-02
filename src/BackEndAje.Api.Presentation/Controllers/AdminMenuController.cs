namespace BackEndAje.Api.Presentation.Controllers
{
    using System.Net;
    using BackEndAje.Api.Application.Dtos;
    using BackEndAje.Api.Application.Menus.Commands.CreateMenuGroup;
    using BackEndAje.Api.Application.Menus.Queries.GetAllMenuGroups;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AdminMenuController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminMenuController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Route("GetAllMenuGroups")]
        public async Task<IActionResult> GetAllMenuGroups()
        {
            var query = new GetAllMenuGroupsQuery();
            var menuGroup = await this._mediator.Send(query);
            return this.Ok(new Response { Result = menuGroup! });
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IDictionary<string, string>), (int)HttpStatusCode.BadRequest)]
        [Route("MenuGroup/Create")]
        public async Task<IActionResult> CreateMenuGroup([FromBody] CreateMenuGroupCommand command)
        {
            var userId = this.GetUserId();
            command.MenuGroup.CreatedBy = userId;
            command.MenuGroup.UpdatedBy = userId;
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
