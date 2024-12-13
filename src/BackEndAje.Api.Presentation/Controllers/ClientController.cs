namespace BackEndAje.Api.Presentation.Controllers
{
    using System.Net;
    using BackEndAje.Api.Application.Clients.Commands.CreateClient;
    using BackEndAje.Api.Application.Clients.Commands.DisableClient;
    using BackEndAje.Api.Application.Clients.Commands.UpdateClient;
    using BackEndAje.Api.Application.Clients.Commands.UploadClient;
    using BackEndAje.Api.Application.Clients.Queries.GetAllClients;
    using BackEndAje.Api.Application.Clients.Queries.GetClientByClientCode;
    using BackEndAje.Api.Application.Dtos;
    using BackEndAje.Api.Application.Dtos.Const;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ClientController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClientController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IDictionary<string, string>), (int)HttpStatusCode.BadRequest)]
        [Route("create")]
        public async Task<IActionResult> CreateClient([FromBody] CreateClientCommand command)
        {
            await this._mediator.Send(command);
            return this.Ok(new { Message = ConstName.MessageOkCreatedResult });
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<GetAllClientsResult>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Route("all")]
        public async Task<IActionResult> GetAllClients([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? filtro = null)
        {
            var query = new GetAllClientsQuery(pageNumber, pageSize, filtro);
            var clients = await this._mediator.Send(query);
            return this.Ok(new Response { Result = clients });
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IDictionary<string, string>), (int)HttpStatusCode.BadRequest)]
        [Route("update")]
        public async Task<IActionResult> UpdateClient([FromBody] UpdateClientCommand command)
        {
            await this._mediator.Send(command);
            return this.Ok(new { Message = ConstName.MessageOkUpdatedResult });
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<GetClientByClientCodeResult>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Route("clientCode/{clientCode}/cediId/{cediId}")]
        public async Task<IActionResult> GetClientByClientCode(int clientCode, int cediId, int? route = null, int? reasonRequestId = null)
        {
            var query = new GetClientByClientCodeQuery(clientCode, cediId, route, reasonRequestId);
            var client = await this._mediator.Send(query);
            return this.Ok(new Response { Result = client });
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IDictionary<string, string>), (int)HttpStatusCode.BadRequest)]
        [Route("updateStatusClient/{clientId}")]
        public async Task<IActionResult> DisableClient(int clientId)
        {
            var command = new DisableClientCommand { ClientId = clientId };
            await this._mediator.Send(command);
            return this.Ok(new { Message = ConstName.GetMessageUpdateClientStatusById(clientId) });
        }

        [HttpPost]
        [Route("upload")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UploadClients(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return this.BadRequest("No hay Clientes uploaded.");
            }

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            var command = new UploadClientsCommand
            {
                FileBytes = memoryStream.ToArray(),
            };
            var result = await this._mediator.Send(command);
            return this.Ok(result);
        }
    }
}
