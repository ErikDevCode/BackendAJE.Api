namespace BackEndAje.Api.Presentation.Controllers
{
    using System.Net;
    using BackEndAje.Api.Application.Census.Commands.CreateCensusAnswer;
    using BackEndAje.Api.Application.Census.Commands.UpdateCensusAnswer;
    using BackEndAje.Api.Application.Census.Queries.GetAnswerByClientId;
    using BackEndAje.Api.Application.Census.Queries.GetCensusQuestions;
    using BackEndAje.Api.Application.Dtos;
    using BackEndAje.Api.Application.Dtos.Const;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
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
            await this._mediator.Send(command);
            return this.Ok(new { Message = ConstName.MessageOkCreatedResult });
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<GetAnswerByClientIdResult>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Route("get-answer-by-clientid")]
        public async Task<IActionResult> GetAnswerByClientId(int? PageNumber = null, int? PageSize = null, int? AssetId = null, int? clientId = null, string? monthPeriod = null)
        {
            var query = new GetAnswerByClientIdQuery(PageNumber, PageSize, AssetId, clientId, monthPeriod);
            var answer = await this._mediator.Send(query);
            return this.Ok(new Response { Result = answer });
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IDictionary<string, string>), (int)HttpStatusCode.BadRequest)]
        [Route("update-answer")]
        public async Task<IActionResult> UpdateCensusAnswer([FromForm] UpdateCensusAnswerCommand command)
        {
            await this._mediator.Send(command);
            return this.Ok(new { Message = ConstName.MessageOkUpdatedResult });
        }
    }
}

