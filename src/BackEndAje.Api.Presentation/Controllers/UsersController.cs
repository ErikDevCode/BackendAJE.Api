namespace BackEndAje.Api.Presentation.Controllers
{
    using System.Net;
    using BackEndAje.Api.Application.Dtos.Users;
    using BackEndAje.Api.Application.Users.Commands.CreateUser;
    using BackEndAje.Api.Application.Users.Commands.UpdateUser;
    using BackEndAje.Api.Application.Users.Commands.UpdateUserPassword;
    using BackEndAje.Api.Application.Users.Queries.GetUser;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    /// <inheritdoc />
    [ApiController]
    [Route("api/[controller]")]
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
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto createUserDto)
        {
            var command = new CreateUserCommand(createUserDto.Username, createUserDto.Email, createUserDto.Password);
            var result = await this._mediator.Send(command);
            return this.Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(CreateUserResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IDictionary<string, string>), (int)HttpStatusCode.BadRequest)]
        [Route("{email}")]
        public async Task<IActionResult> GetUser(string email)
        {
            var query = new GetUserQuery(email);
            var result = await this._mediator.Send(query);
            return this.Ok(result);
        }

        [HttpPut]
        [ProducesResponseType(typeof(CreateUserResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IDictionary<string, string>), (int)HttpStatusCode.BadRequest)]
        [Route("update")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto updateUserDto)
        {
            var command = new UpdateUserCommand(updateUserDto.Username, updateUserDto.Email);
            var result = await this._mediator.Send(command);
            return this.Ok(result);
        }

        [HttpPut]
        [ProducesResponseType(typeof(CreateUserResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IDictionary<string, string>), (int)HttpStatusCode.BadRequest)]
        [Route("update-password")]
        public async Task<IActionResult> UpdateUserPasswordByEmail([FromBody] UpdateUserPasswordDto updateUserPasswordDto)
        {
            if (string.IsNullOrEmpty(updateUserPasswordDto.Email) || string.IsNullOrEmpty(updateUserPasswordDto.NewPassword))
            {
                return this.BadRequest("Email and Password cannot be empty.");
            }

            var command = new UpdateUserPasswordByEmailCommand(updateUserPasswordDto.Email, updateUserPasswordDto.NewPassword);
            var result = await this._mediator.Send(command);
            return this.Ok(result);
        }
    }
}
