namespace BackEndAje.Api.Presentation.Controllers
{
    using System.Net;
    using BackEndAje.Api.Application.Asset.Command.CreateAsset;
    using BackEndAje.Api.Application.Asset.Command.UpdateAsset;
    using BackEndAje.Api.Application.Asset.Command.UpdateStatusAsset;
    using BackEndAje.Api.Application.Asset.Queries.GetAllAssets;
    using BackEndAje.Api.Application.Asset.Queries.GetAssetsByCodeAje;
    using BackEndAje.Api.Application.Dtos;
    using BackEndAje.Api.Application.Exceptions;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
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
        public async Task<IActionResult> GetAllAssets([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetAllAssetsQuery(pageNumber, pageSize);
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

