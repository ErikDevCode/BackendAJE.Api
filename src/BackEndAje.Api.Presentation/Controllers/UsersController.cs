﻿namespace BackEndAje.Api.Presentation.Controllers
{
    using System.Net;
    using BackEndAje.Api.Application.Dtos;
    using BackEndAje.Api.Application.Dtos.Const;
    using BackEndAje.Api.Application.Users.Commands.CreateUser;
    using BackEndAje.Api.Application.Users.Commands.UpdateStatusUser;
    using BackEndAje.Api.Application.Users.Commands.UpdateUser;
    using BackEndAje.Api.Application.Users.Commands.UploadUsers;
    using BackEndAje.Api.Application.Users.Queries.GetAllUser;
    using BackEndAje.Api.Application.Users.Queries.GetMenuForUserById;
    using BackEndAje.Api.Application.Users.Queries.GetSupervisorByCedi;
    using BackEndAje.Api.Application.Users.Queries.GetUserById;
    using BackEndAje.Api.Application.Users.Queries.GetUserByRouteOrEmail;
    using BackEndAje.Api.Application.Users.Queries.GetUsersByParam;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

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
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Route("create")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
        {
            await this._mediator.Send(command);
            return this.Ok(new { Message = ConstName.MessageOkCreatedResult });
        }

        [HttpGet]
        [ProducesResponseType(typeof(GetAllUserResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IDictionary<string, string>), (int)HttpStatusCode.BadRequest)]
        [Route("all")]
        public async Task<IActionResult> GetAllUser([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? filtro = null)
        {
            var query = new GetAllUserQuery(pageNumber, pageSize, filtro);
            var result = await this._mediator.Send(query);
            return this.Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(GetUsersByParamResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IDictionary<string, string>), (int)HttpStatusCode.BadRequest)]
        [Route("SearchBoxUsers")]
        public async Task<IActionResult> GetUsersByParam([FromQuery] string? param = null)
        {
            var query = new GetUsersByParamQuery(param ?? string.Empty);
            var result = await this._mediator.Send(query);
            return this.Ok(new Response { Result = result });
        }

        [HttpGet]
        [ProducesResponseType(typeof(GetUserByRouteOrEmailResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IDictionary<string, string>), (int)HttpStatusCode.BadRequest)]
        [Route("{routeOrEmail}")]
        public async Task<IActionResult> GetUserByRouteOrEmail(string routeOrEmail)
        {
            var query = new GetUserByRouteOrEmailQuery(routeOrEmail);
            var result = await this._mediator.Send(query);
            return this.Ok(new Response { Result = result });
        }

        [HttpGet]
        [ProducesResponseType(typeof(GetUserByIdResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IDictionary<string, string>), (int)HttpStatusCode.BadRequest)]
        [Route("GetUserById/{id:int}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var query = new GetUserByIdQuery(id);
            var result = await this._mediator.Send(query);
            return this.Ok(new Response { Result = result });
        }

        [HttpPut]
        [ProducesResponseType(typeof(UpdateUserResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IDictionary<string, string>), (int)HttpStatusCode.BadRequest)]
        [Route("update")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserCommand command)
        {
            await this._mediator.Send(command);
            return this.Ok(new { Message = ConstName.MessageOkUpdatedResult });
        }

        [HttpPatch]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IDictionary<string, string>), (int)HttpStatusCode.BadRequest)]
        [Route("update-status-user")]
        public async Task<IActionResult> UpdateStatusUser([FromBody] UpdateStatusUserCommand command)
        {
            await this._mediator.Send(command);
            return this.Ok(new { Message = ConstName.MessageUpdateUserById(command.UserId) });
        }

        [HttpGet]
        [ProducesResponseType(typeof(GetMenuForUserByIdResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IDictionary<string, string>), (int)HttpStatusCode.BadRequest)]
        [Route("menu-for-user")]
        public async Task<IActionResult> GetMenuForUser()
        {
            var query = new GetMenuForUserByIdQuery();
            var result = await this._mediator.Send(query);
            return this.Ok(result);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Route("upload")]
        public async Task<IActionResult> UploadUsers(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return this.BadRequest("No file uploaded.");
            }

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            var command = new UploadUsersCommand
            {
                FileBytes = memoryStream.ToArray(),
            };
            var result = await this._mediator.Send(command);
            return this.Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(GetSupervisorByCediResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IDictionary<string, string>), (int)HttpStatusCode.BadRequest)]
        [Route("GetSupervisorByCediId/{cediId}")]
        public async Task<IActionResult> GetSupervisorByCedi(int cediId)
        {
            var query = new GetSupervisorByCediQuery(cediId);
            var result = await this._mediator.Send(query);
            return this.Ok(new Response { Result = result });
        }
    }
}
