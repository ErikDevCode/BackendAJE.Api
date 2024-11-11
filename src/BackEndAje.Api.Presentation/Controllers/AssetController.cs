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
    using BackEndAje.Api.Application.Asset.Queries.GetClientAssets;
    using BackEndAje.Api.Application.Asset.Queries.GetClientAssetsTrace;
    using BackEndAje.Api.Application.Dtos;
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
            var userId = this.GetUserId();
            command.CreatedBy = userId;
            command.UpdatedBy = userId;
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

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IDictionary<string, string>), (int)HttpStatusCode.BadRequest)]
        [Route("update")]
        public async Task<IActionResult> UpdateAsset([FromBody] UpdateAssetCommand command)
        {
            var userId = this.GetUserId();
            command.UpdatedBy = userId;
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
            var userId = this.GetUserId();
            command.UpdatedBy = userId;
            var result = await this._mediator.Send(command);
            if (result)
            {
                return this.Ok(new { Message = $"Activo con ID: '{command.AssetId}' fue actualizado satisfactoriamente." });
            }

            return this.NotFound(new { Message = $"Activo con ID: '{command.AssetId}' no encontrado o ya se encuentra eliminado." });
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

            var userId = this.GetUserId();
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            var command = new UploadAssetsCommand
            {
                FileBytes = memoryStream.ToArray(),
                CreatedBy = userId,
                UpdatedBy = userId,
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
            var userId = this.GetUserId();
            command.CreatedBy = userId;
            command.UpdatedBy = userId;
            var result = await this._mediator.Send(command);
            return this.Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<GetClientAssetsResult>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Route("client-asset/all")]
        public async Task<IActionResult> GetClientAssets([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? codeAje = null, [FromQuery] int? clientId = null, [FromQuery] int? userId = null)
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
            var userId = this.GetUserId();
            command.UpdatedBy = userId;
            var result = await this._mediator.Send(command);
            return this.Ok(result);
        }

        [HttpPatch]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IDictionary<string, string>), (int)HttpStatusCode.BadRequest)]
        [Route("client-asset/update-deactivate-activate-clientasset")]
        public async Task<IActionResult> UpdateDeactivateClientAsset([FromBody] UpdateDeactivateClientAssetCommand command)
        {
            var userId = this.GetUserId();
            command.UpdatedBy = userId;
            var result = await this._mediator.Send(command);
            if (result)
            {
                return this.Ok(new { Message = $"Se actualizó satisfactoriamente." });
            }

            return this.NotFound(new { Message = $"No se encuentra" });
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IDictionary<string, string>), (int)HttpStatusCode.BadRequest)]
        [Route("client-asset/reassign")]
        public async Task<IActionResult> UpdateClientAssetReassign([FromBody] UpdateClientAssetReassignCommand command)
        {
            var userId = this.GetUserId();
            command.UpdatedBy = userId;
            var result = await this._mediator.Send(command);
            return this.Ok(result);
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

