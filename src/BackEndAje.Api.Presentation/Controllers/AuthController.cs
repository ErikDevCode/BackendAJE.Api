﻿namespace BackEndAje.Api.Presentation.Controllers
{
    using System.Net;
    using BackEndAje.Api.Application.Users.Commands.CreateUser;
    using BackEndAje.Api.Application.Users.Commands.LoginUser;
    using BackEndAje.Api.Application.Users.Commands.UpdateUserPassword;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(typeof(LoginUserResult),  (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {
            var result = await this._mediator.Send(command);
            return this.Ok(result);
        }

        [HttpPut]
        [ProducesResponseType(typeof(CreateUserResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IDictionary<string, string>), (int)HttpStatusCode.BadRequest)]
        [Route("update-password")]
        public async Task<IActionResult> UpdateUserPasswordByEmail([FromBody] UpdateUserPasswordByEmailCommand command)
        {
            var result = await this._mediator.Send(command);
            return this.Ok(result);
        }
    }
}
