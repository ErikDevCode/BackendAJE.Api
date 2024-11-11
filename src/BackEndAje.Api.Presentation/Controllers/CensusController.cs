using BackEndAje.Api.Application.Census.Commands.CreateCensusAnswer;

namespace BackEndAje.Api.Presentation.Controllers
{
    using System.Net;
    using BackEndAje.Api.Application.Census.Queries.GetCensusQuestions;
    using BackEndAje.Api.Application.Dtos;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class CensusController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CensusController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<GetCensusQuestionsResult>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Route("census-questions")]
        public async Task<IActionResult> GetCensusQuestions(int clientId)
        {
            var query = new GetCensusQuestionsQuery(clientId);
            var censusQuestions = await this._mediator.Send(query);
            return this.Ok(new Response { Result = censusQuestions });
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IDictionary<string, string>), (int)HttpStatusCode.BadRequest)]
        [Route("create-answer")]
        public async Task<IActionResult> CreateCensusAnswer([FromForm] CreateCensusAnswerCommand command)
        {
            var userId = this.GetUserId();
            command.CreatedBy = userId;
            var result = await this._mediator.Send(command);
            return this.Ok(result);
        }

        private int GetUserId()
        {
            var userIdClaim = this.User.FindFirst("UserId") ?? this.User.FindFirst("sub");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                throw new UnauthorizedAccessException("Usuario ID no encontrado o token invalido.");
            }

            return userId;
        }
    }
}

