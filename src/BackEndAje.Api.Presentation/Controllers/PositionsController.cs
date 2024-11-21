namespace BackEndAje.Api.Presentation.Controllers
{
    using System.Net;
    using BackEndAje.Api.Application.Dtos;
    using BackEndAje.Api.Application.Dtos.Const;
    using BackEndAje.Api.Application.Positions.Commands.CreatePosition;
    using BackEndAje.Api.Application.Positions.Commands.UpdatePosition;
    using BackEndAje.Api.Application.Positions.Commands.UpdateStatusPosition;
    using BackEndAje.Api.Application.Positions.Queries.GetAllPositions;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]

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
            var result = await this._mediator.Send(command);
            return this.Ok(result);
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Route("update")]
        public async Task<IActionResult> UpdatePosition([FromBody] UpdatePositionCommand command)
        {
            var result = await this._mediator.Send(command);
            return this.Ok(result);
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Route("updateStatus")]
        public async Task<IActionResult> UpdateStatusPosition(int positionId)
        {
            var command = new UpdateStatusPositionCommand { PositionId = positionId };
            var result = await this._mediator.Send(command);
            if (result)
            {
                return this.Ok(new { Message = $"Cargo con ID: '{positionId}' fue actualizado correctamente." });
            }

            return this.NotFound(new { Message = $"Cargo con ID: '{positionId}' no encontrado o ya se encuentra eliminado." });
        }
    }
}

