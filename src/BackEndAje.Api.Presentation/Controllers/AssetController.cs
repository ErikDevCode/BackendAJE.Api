using BackEndAje.Api.Application.Asset.Command.UploadClientAssets;

namespace BackEndAje.Api.Presentation.Controllers
{
    using System.Net;
    using BackEndAje.Api.Application.Asset.Command.CreateAsset;
    using BackEndAje.Api.Application.Asset.Command.CreateClientAsset;
    using BackEndAje.Api.Application.Asset.Command.UpdateAsset;
    using BackEndAje.Api.Application.Asset.Command.UpdateClientAsset;
    using BackEndAje.Api.Application.Asset.Command.UpdateClientAssetReassign;
    using BackEndAje.Api.Application.Asset.Command.UpdateDeactivateClientAsset;
    using BackEndAje.Api.Application.Asset.Command.UpdateStatusAsset;
    using BackEndAje.Api.Application.Asset.Command.UploadAssets;
    using BackEndAje.Api.Application.Asset.Queries.GetAllAssets;
    using BackEndAje.Api.Application.Asset.Queries.GetAssetsByCodeAje;
    using BackEndAje.Api.Application.Asset.Queries.GetAssetWithOutClient;
    using BackEndAje.Api.Application.Asset.Queries.GetClientAssets;
    using BackEndAje.Api.Application.Asset.Queries.GetClientAssetsTrace;
    using BackEndAje.Api.Application.Dtos;
    using BackEndAje.Api.Application.Dtos.Const;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AssetController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AssetController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IDictionary<string, string>), (int)HttpStatusCode.BadRequest)]
        [Route("create")]
        public async Task<IActionResult> CreateAsset([FromBody] CreateAssetCommand command)
        {
            var result = await this._mediator.Send(command);
            return this.Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<GetAllAssetsResult>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Route("all")]
        public async Task<IActionResult> GetAllAssets([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? codeAje = null)
        {
            var query = new GetAllAssetsQuery(pageNumber, pageSize, codeAje);
            var clients = await this._mediator.Send(query);
            return this.Ok(new Response { Result = clients });
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<GetAssetWithOutClientResult>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Route("asset-without-client")]
        public async Task<IActionResult> GetAssetWithOutClient([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? codeAje = null)
        {
            var query = new GetAssetWithOutClientQuery(pageNumber, pageSize, codeAje);
            var clients = await this._mediator.Send(query);
            return this.Ok(new Response { Result = clients });
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IDictionary<string, string>), (int)HttpStatusCode.BadRequest)]
        [Route("update")]
        public async Task<IActionResult> UpdateAsset([FromBody] UpdateAssetCommand command)
        {
            var result = await this._mediator.Send(command);
            return this.Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<GetAssetsByCodeAjeResult>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Route("byCodeAje/{codeAje}")]
        public async Task<IActionResult> GetAssetsByCodeAje(string codeAje)
        {
            var query = new GetAssetsByCodeAjeQuery(codeAje);
            var client = await this._mediator.Send(query);
            return this.Ok(new Response { Result = client });
        }

        [HttpPatch]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IDictionary<string, string>), (int)HttpStatusCode.BadRequest)]
        [Route("updateStatusAsset")]
        public async Task<IActionResult> UpdateStatusAsset([FromBody] UpdateStatusAssetCommand command)
        {
            await this._mediator.Send(command);
            return this.Ok(new { Message = ConstName.GetMessageUpdateStatusById(command.AssetId) });
        }

        [HttpPost]
        [Route("upload")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UploadAssets(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return this.BadRequest("No hay Activos uploaded.");
            }

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            var command = new UploadAssetsCommand
            {
                FileBytes = memoryStream.ToArray(),
            };
            var result = await this._mediator.Send(command);
            return this.Ok(result);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IDictionary<string, string>), (int)HttpStatusCode.BadRequest)]
        [Route("client-asset/create-client-asset")]
        public async Task<IActionResult> CreateClientAsset([FromBody] CreateClientAssetCommand command)
        {
            await this._mediator.Send(command);
            return this.Ok(new { Message = ConstName.MessageOkCreatedResult });
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<GetClientAssetsResult>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Route("client-asset/all")]
        public async Task<IActionResult> GetClientAssets([FromQuery] int? pageNumber = null, [FromQuery] int? pageSize = null, [FromQuery] string? codeAje = null, [FromQuery] int? clientId = null, [FromQuery] int? userId = null)
        {
            var query = new GetClientAssetsQuery(pageNumber, pageSize, codeAje, clientId, userId);
            var clientAsset = await this._mediator.Send(query);
            return this.Ok(new Response { Result = clientAsset });
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IDictionary<string, string>), (int)HttpStatusCode.BadRequest)]
        [Route("client-asset/update")]
        public async Task<IActionResult> UpdateClientAsset([FromBody] UpdateClientAssetCommand command)
        {
            await this._mediator.Send(command);
            return this.Ok(new { Message = ConstName.MessageOkUpdatedResult });
        }

        [HttpPatch]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IDictionary<string, string>), (int)HttpStatusCode.BadRequest)]
        [Route("client-asset/update-deactivate-activate-clientasset")]
        public async Task<IActionResult> UpdateDeactivateClientAsset([FromBody] UpdateDeactivateClientAssetCommand command)
        {
            await this._mediator.Send(command);
            return this.Ok(new { Message = ConstName.MessageOkUpdatedResult });
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IDictionary<string, string>), (int)HttpStatusCode.BadRequest)]
        [Route("client-asset/reassign")]
        public async Task<IActionResult> UpdateClientAssetReassign([FromBody] UpdateClientAssetReassignCommand command)
        {
            await this._mediator.Send(command);
            return this.Ok(new { Message = ConstName.MessageOkUpdatedResult });
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<GetClientAssetsTraceResult>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Route("client-asset-trace/by-asset-id")]
        public async Task<IActionResult> GetClientAssetsTrace([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] int? assetId = null)
        {
            var query = new GetClientAssetsTraceQuery(pageNumber, pageSize, assetId);
            var clientAsset = await this._mediator.Send(query);
            return this.Ok(new Response { Result = clientAsset });
        }

        [HttpPost]
        [Route("client-asset/upload")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UploadClientAssets(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return this.BadRequest("No hay Activos uploaded.");
            }

            try
            {
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);

                var command = new UploadClientAssetsCommand
                {
                    FileBytes = memoryStream.ToArray(),
                };

                await this._mediator.Send(command);
                return this.Ok("Carga de activos completada exitosamente.");
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
    }
}

