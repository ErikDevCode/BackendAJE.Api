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
            var userId = this.GetUserId();
            command.CreatedBy = userId;
            command.UpdatedBy = userId;
            var result = await this._mediator.Send(command);
            return this.Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<GetAllClientsResult>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Route("all")]
        public async Task<IActionResult> GetAllClients([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetAllClientsQuery(pageNumber, pageSize);
            var clients = await this._mediator.Send(query);
            return this.Ok(new Response { Result = clients });
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IDictionary<string, string>), (int)HttpStatusCode.BadRequest)]
        [Route("update")]
        public async Task<IActionResult> UpdateClient([FromBody] UpdateClientCommand command)
        {
            var userId = this.GetUserId();
            command.UpdatedBy = userId;
            var result = await this._mediator.Send(command);
            return this.Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<GetClientByClientCodeResult>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Route("clientCode/{clientCode}/cediId/{cediId}")]
        public async Task<IActionResult> GetClientByClientCode(int clientCode, int cediId)
        {
            var query = new GetClientByClientCodeQuery(clientCode, cediId);
            var client = await this._mediator.Send(query);
            return this.Ok(new Response { Result = client });
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IDictionary<string, string>), (int)HttpStatusCode.BadRequest)]
        [Route("updateStatusClient/{clientId}")]
        public async Task<IActionResult> DisableClient(int clientId)
        {
            var userId = this.GetUserId();
            var command = new DisableClientCommand { ClientId = clientId, UpdatedBy = userId };
            var result = await this._mediator.Send(command);
            if (result)
            {
                return this.Ok(new { Message = $"Cliente con ID: '{command.ClientId}' fue actualizado satisfactoriamente." });
            }

            return this.NotFound(new { Message = $"Cliente con ID: '{command.ClientId}' no encontrado o ya se encuentra eliminado." });
        }

        [HttpPost]
        [Route("upload")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UploadClients(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return this.BadRequest("No hay documento uploaded.");
            }

            var userId = this.GetUserId();
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            var command = new UploadClientsCommand
            {
                FileBytes = memoryStream.ToArray(),
                CreatedBy = userId,
                UpdatedBy = userId,
            };
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
