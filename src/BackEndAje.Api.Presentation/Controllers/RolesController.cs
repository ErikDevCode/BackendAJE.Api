namespace BackEndAje.Api.Presentation.Controllers
{
    using System.Net;
    using BackEndAje.Api.Application.Dtos;
    using BackEndAje.Api.Application.Dtos.Roles;
    using BackEndAje.Api.Application.Roles.Commands.AssignPermissionToRole;
    using BackEndAje.Api.Application.Roles.Commands.CreateRole;
    using BackEndAje.Api.Application.Roles.Commands.UpdateRole;
    using BackEndAje.Api.Application.Roles.Commands.UpdateStatusRole;
    using BackEndAje.Api.Application.Roles.Queries.GetAllRoles;
    using BackEndAje.Api.Application.Roles.Queries.GetAllRolesWithPermissions;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RolesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RolesController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<GetAllRolesResult>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAllRoles([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetAllRolesQuery(pageNumber, pageSize);
            var roles = await this._mediator.Send(query);
            return this.Ok(new Response { Result = roles });
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IDictionary<string, string>), (int)HttpStatusCode.BadRequest)]
        [Route("create")]
        public async Task<IActionResult> CreateRole([FromBody] CreateRolesCommand command)
        {
            var userId = this.GetUserId();
            command.Role.CreatedBy = userId;
            command.Role.UpdatedBy = userId;
            var result = await this._mediator.Send(command);
            return this.Ok(result);
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Route("update")]
        public async Task<IActionResult> UpdateRole([FromBody] UpdateRolesCommand command)
        {
            var userId = this.GetUserId();
            command.Role.UpdatedBy = userId;
            var result = await this._mediator.Send(command);
            return this.Ok(result);
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Route("updateStatus/{roleId}")]
        public async Task<IActionResult> UpdateStatusRole(int roleId)
        {
            var userId = this.GetUserId();
            var command = new UpdateStatusRoleCommand { RoleUpdateStatus = new UpdateStatusRoleDto() { RoleId = roleId, UpdatedBy = userId } };
            var result = await this._mediator.Send(command);
            if (result)
            {
                return this.Ok(new { Message = $"RoleId '{roleId}' has been update status successfully." });
            }

            return this.NotFound(new { Message = $"RoleId '{roleId}' not found or already deleted." });
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IDictionary<string, string>), (int)HttpStatusCode.BadRequest)]
        [Route("AssignPermissionsToRole")]
        public async Task<IActionResult> AssignPermissionsToRole([FromBody] AssignPermissionToRoleCommand command)
        {
            var userId = this.GetUserId();
            command.AssignPermissionToRole.CreatedBy = userId;
            command.AssignPermissionToRole.UpdatedBy = userId;
            var result = await this._mediator.Send(command);
            return this.Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<GetAllRolesWithPermissionsResult>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Route("GetRolesWithPermissions")]
        public async Task<IActionResult> GetAllRolesWithPermissions()
        {
            var query = new GetAllRolesWithPermissionsQuery();
            var roles = await this._mediator.Send(query);
            return this.Ok(new Response { Result = roles });
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
