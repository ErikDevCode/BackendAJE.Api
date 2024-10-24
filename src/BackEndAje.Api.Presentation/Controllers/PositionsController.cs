namespace BackEndAje.Api.Presentation.Controllers
{
    using System.Net;
    using BackEndAje.Api.Application.Dtos;
    using BackEndAje.Api.Application.Positions.Commands.CreatePosition;
    using BackEndAje.Api.Application.Positions.Commands.UpdatePosition;
    using BackEndAje.Api.Application.Positions.Commands.UpdateStatusPosition;
    using BackEndAje.Api.Application.Positions.Queries.GetAllPositions;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class PositionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PositionsController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<GetAllPositionsResult>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAllPositions([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetAllPositionsQuery(pageNumber, pageSize);
            var positions = await this._mediator.Send(query);
            return this.Ok(new Response { Result = positions });
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IDictionary<string, string>), (int)HttpStatusCode.BadRequest)]
        [Route("create")]
        public async Task<IActionResult> CreatePosition([FromBody] CreatePositionCommand command)
        {
            var userId = this.GetUserId();
            command.CreatedBy = userId;
            command.UpdatedBy = userId;
            var result = await this._mediator.Send(command);
            return this.Ok(result);
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Route("update")]
        public async Task<IActionResult> UpdatePosition([FromBody] UpdatePositionCommand command)
        {
            var userId = this.GetUserId();
            command.UpdatedBy = userId;
            var result = await this._mediator.Send(command);
            return this.Ok(result);
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Route("updateStatus/{roleId}")]
        public async Task<IActionResult> UpdateStatusPosition(int positionId)
        {
            var userId = this.GetUserId();
            var command = new UpdateStatusPositionCommand { PositionId = positionId, UpdatedBy = userId };
            var result = await this._mediator.Send(command);
            if (result)
            {
                return this.Ok(new { Message = $"PositionId '{positionId}' has been update status successfully." });
            }

            return this.NotFound(new { Message = $"PositionId '{positionId}' not found or already deleted." });
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

