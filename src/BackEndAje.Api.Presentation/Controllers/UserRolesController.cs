﻿namespace BackEndAje.Api.Presentation.Controllers
{
    using System.Net;
    using BackEndAje.Api.Application.Dtos;
    using BackEndAje.Api.Application.Dtos.Const;
    using BackEndAje.Api.Application.Users.Commands.AssignRolesToUser;
    using BackEndAje.Api.Application.Users.Commands.RemoveRoleToUser;
    using BackEndAje.Api.Application.Users.Queries.GetUserRolesById;
    using BackEndAje.Api.Application.Users.Queries.GetUsersWithRoles;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserRolesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserRolesController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Route("assign-role")]
        public async Task<IActionResult> AssignRoleToUser([FromBody] AssignRolesToUserCommand command)
        {
            await this._mediator.Send(command);
            return this.Ok(new { Message = ConstName.MessageOkAssignResult });
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<GetUserRolesByIdResult>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Route("{userId}/roles")]
        public async Task<IActionResult> GetUserRolesById(int userId)
        {
            var query = new GetUserRolesByIdQuery(userId);
            var roles = await this._mediator.Send(query);
            return this.Ok(new Response { Result = roles });
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<GetUsersWithRolesResult>), (int)HttpStatusCode.OK)]
        [Route("all-with-roles")]
        public async Task<IActionResult> GetAllUsersWithRoles()
        {
            var query = new GetUsersWithRolesQuery();
            var usersWithRoles = await this._mediator.Send(query);

            return this.Ok(new Response { Result = usersWithRoles });
        }

        [HttpDelete]
        [Route("{userId}/roles/{roleId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RemoveRoleFromUser(int userId, int roleId)
        {
            var command = new RemoveRolesToUserCommand(userId, roleId);
            await this._mediator.Send(command);
            return this.Ok(new { Message = ConstName.MessageOkUpdatedResult });
        }
    }
}