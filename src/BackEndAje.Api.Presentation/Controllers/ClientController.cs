namespace BackEndAje.Api.Presentation.Controllers
{
    using System.Net;
    using BackEndAje.Api.Application.Clients.Commands.CreateClient;
    using BackEndAje.Api.Application.Clients.Commands.DisableClient;
    using BackEndAje.Api.Application.Clients.Commands.UpdateClient;
    using BackEndAje.Api.Application.Clients.Commands.UploadClient;
    using BackEndAje.Api.Application.Clients.Commands.UploadCodeAndNameClients;
    using BackEndAje.Api.Application.Clients.Commands.UploadDeleteClients;
    using BackEndAje.Api.Application.Clients.Queries.GetAllClients;
    using BackEndAje.Api.Application.Clients.Queries.GetClientByClientCode;
    using BackEndAje.Api.Application.Clients.Queries.GetExportClient;
    using BackEndAje.Api.Application.Clients.Queries.GetListClientByClientCode;
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

        [HttpGet]
        [ProducesResponseType(typeof(List<GetClientByClientCodeResult>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Route("clientCode/{clientCode}")]
        public async Task<IActionResult> GetListClientByClientCode(int clientCode)
        {
            var query = new GetListClientByClientCodeQuery(clientCode);
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

        [HttpGet]
        [Route("export")]
        [ProducesResponseType(typeof(FileContentResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ExportClients()
        {
            try
            {
                var query = new ExportClientsQuery();
                var fileContent = await this._mediator.Send(query);
                var fileName = $"Clientes_{DateTime.Now:yyyyMMddHHmmss}.xlsx";

                return this.File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost]
        [Route("uploadCodeAndNameClients")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UploadCodeAndNameClients(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return this.BadRequest("No hay Clientes uploaded.");
            }

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            var command = new UploadCodeAndNameClientsCommand()
            {
                FileBytes = memoryStream.ToArray(),
            };
            var result = await this._mediator.Send(command);
            return this.Ok(result);
        }

        [HttpPost]
        [Route("deleteClients")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UploadDeleteClients(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return this.BadRequest("No hay Clientes uploaded.");
            }

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            var command = new UploadDeleteClientsCommand()
            {
                FileBytes = memoryStream.ToArray(),
            };
            var result = await this._mediator.Send(command);
            return this.Ok(result);
        }
    }
}
