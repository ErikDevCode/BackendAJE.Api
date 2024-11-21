namespace BackEndAje.Api.Presentation.Controllers
{
    using System.Net;
    using BackEndAje.Api.Application.Dtos;
    using BackEndAje.Api.Application.Dtos.Const;
    using BackEndAje.Api.Application.Dtos.Roles;
    using BackEndAje.Api.Application.Roles.Commands.AssignPermissionsWithActions;
    using BackEndAje.Api.Application.Roles.Commands.AssignPermissionToRole;
    using BackEndAje.Api.Application.Roles.Commands.CreateRole;
    using BackEndAje.Api.Application.Roles.Commands.UpdateRole;
    using BackEndAje.Api.Application.Roles.Commands.UpdateStatusRole;
    using BackEndAje.Api.Application.Roles.Queries.GetAllRoles;
    using BackEndAje.Api.Application.Roles.Queries.GetAllRolesWithPermissions;
    using BackEndAje.Api.Application.Roles.Queries.GetPermissionsWithActionByRoleId;
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
            var result = await this._mediator.Send(command);
            return this.Ok(result);
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Route("update")]
        public async Task<IActionResult> UpdateRole([FromBody] UpdateRolesCommand command)
        {
            var result = await this._mediator.Send(command);
            return this.Ok(result);
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Route("updateStatus/{roleId}")]
        public async Task<IActionResult> UpdateStatusRole(int roleId)
        {
            var command = new UpdateStatusRoleCommand { RoleUpdateStatus = new UpdateStatusRoleDto() { RoleId = roleId } };
            await this._mediator.Send(command);
            return this.Ok(new { Message = ConstName.MessageUpdateRoleById(roleId) });
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IDictionary<string, string>), (int)HttpStatusCode.BadRequest)]
        [Route("AssignPermissionsToRole")]
        public async Task<IActionResult> AssignPermissionsToRole([FromBody] AssignPermissionToRoleCommand command)
        {
            await this._mediator.Send(command);
            return this.Ok(new { Message = ConstName.MessageOkAssignResult });
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<GetAllRolesWithPermissionsResult>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Route("GetRolesWithPermissions")]
        public async Task<IActionResult> GetAllRolesWithPermissions(int? roleId)
        {
            var query = new GetAllRolesWithPermissionsQuery(roleId);
            var roles = await this._mediator.Send(query);
            return this.Ok(new Response { Result = roles });
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IDictionary<string, string>), (int)HttpStatusCode.BadRequest)]
        [Route("AssignPermissionsWithActions")]
        public async Task<IActionResult> AssignPermissionsWithActions([FromBody] AssignPermissionsWithActionsCommand command)
        {
            await this._mediator.Send(command);
            return this.Ok(new { Message = ConstName.MessageOkAssignResult });
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<GetPermissionsWithActionByRoleIdResult>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Route("GetPermissionsWithActionByRoleId/{roleId}")]
        public async Task<IActionResult> GetPermissionsWithActionByRoleId(int roleId)
        {
            var query = new GetPermissionsWithActionByRoleIdQuery(roleId);
            var permissionsWithAction = await this._mediator.Send(query);
            return this.Ok(new Response { Result = permissionsWithAction });
        }
    }
}
