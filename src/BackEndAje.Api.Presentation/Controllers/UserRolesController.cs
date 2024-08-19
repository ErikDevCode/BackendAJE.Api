namespace BackEndAje.Api.Presentation.Controllers
{
    using System.Net;
    using BackEndAje.Api.Application.Users.Commands.AssignRolesToUser;
    using BackEndAje.Api.Application.Users.Queries.GetUserRolesById;
    using BackEndAje.Api.Application.Users.Queries.GetUsersWithRoles;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class UserRolesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserRolesController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpPost]
        [Route("assign-role")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AssignRoleToUser([FromBody] AssignRolesToUserCommand command)
        {
            var result = await this._mediator.Send(command);
            return this.Ok(result);
        }

        [HttpGet]
        [Route("{userId}/roles")]
        [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetUserRolesById(int userId)
        {
            var query = new GetUserRolesByIdQuery(userId);
            var roles = await this._mediator.Send(query);
            if (roles.Count == 0)
            {
                return this.NotFound($"No roles found for user with ID {userId}.");
            }

            return this.Ok(roles);
        }

        [HttpGet]
        [Route("all-with-roles")]
        [ProducesResponseType(typeof(List<GetUsersWithRolesQuery>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllUsersWithRoles()
        {
            var query = new GetUsersWithRolesQuery();
            var usersWithRoles = await this._mediator.Send(query);

            return this.Ok(usersWithRoles);
        }
    }
}