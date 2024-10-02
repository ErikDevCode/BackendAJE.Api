namespace BackEndAje.Api.Presentation.Controllers
{
    using System.Net;
    using BackEndAje.Api.Application.Dtos;
    using BackEndAje.Api.Application.Dtos.Permissions;
    using BackEndAje.Api.Application.Permissions.Commands.CreatePermission;
    using BackEndAje.Api.Application.Permissions.Commands.DeletePermission;
    using BackEndAje.Api.Application.Permissions.Commands.UpdatePermission;
    using BackEndAje.Api.Application.Permissions.Queries.GetAllPermissions;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AdminPermissionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminPermissionsController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Route("GetAllPermissions")]
        public async Task<IActionResult> GetAllPermissions()
        {
            var query = new GetAllPermissionsQuery();
            var permissions = await this._mediator.Send(query);
            return this.Ok(new Response { Result = permissions });
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IDictionary<string, string>), (int)HttpStatusCode.BadRequest)]
        [Route("create")]
        public async Task<IActionResult> CreatePermission([FromBody] CreatePermissionCommand command)
        {
            var userId = this.GetUserId();
            command.Permission.CreatedBy = userId;
            command.Permission.UpdatedBy = userId;
            var result = await this._mediator.Send(command);
            return this.Ok(result);
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Route("update")]
        public async Task<IActionResult> UpdatePermission([FromBody] UpdatePermissionCommand command)
        {
            var userId = this.GetUserId();
            command.Permission.UpdatedBy = userId;
            var result = await this._mediator.Send(command);
            return this.Ok(result);
        }

        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Route("delete/{permissionId}")]
        public async Task<IActionResult> DeletePermission(int permissionId)
        {
            var userId = this.GetUserId();
            var command = new DeletePermissionCommand { DeletePermission = new DeletePermissionDto() { PermissionId = permissionId, UpdatedBy = userId } };
            var result = await this._mediator.Send(command);
            if (result)
            {
                return this.Ok(new { Message = $"RoleId '{permissionId}' has been deleted successfully." });
            }

            return this.NotFound(new { Message = $"RoleId '{permissionId}' not found or already deleted." });
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
