namespace BackEndAje.Api.Presentation.Controllers
{
    using BackEndAje.Api.Application.Users.Commands.LoginUser;
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
        [ProducesResponseType(typeof(LoginUserResult), 200)]
        [ProducesResponseType(401)]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {
            var result = await this._mediator.Send(command);
            return this.Ok(result);
        }
    }
}
