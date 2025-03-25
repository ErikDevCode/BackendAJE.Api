namespace BackEndAje.Api.Application.Census.Commands.CreateCensusAnswer
{
    using BackEndAje.Api.Application.Interfaces;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class CreateCensusAnswerHandler : IRequestHandler<CreateCensusAnswerCommand, Unit>
    {
        private readonly ICensusRepository _censusRepository;
        private readonly IS3Service _s3Service;
        private readonly IClientAssetRepository _clientAssetRepository;
        private readonly IAssetRepository _assetRepository;

        public CreateCensusAnswerHandler(ICensusRepository censusRepository, IS3Service s3Service, IClientAssetRepository clientAssetRepository, IAssetRepository assetRepository)
        {
            this._censusRepository = censusRepository;
            this._s3Service = s3Service;
            this._clientAssetRepository = clientAssetRepository;
            this._assetRepository = assetRepository;
        }

        public async Task<Unit> Handle(CreateCensusAnswerCommand request, CancellationToken cancellationToken)
        {
            var monthPeriod = DateTime.Now.ToString("yyyyMM");

            var clientAssetById = await this._clientAssetRepository.GetClientAssetByIdAsync(request.ClientAssetId);

            var clientAssetValidation = await this._clientAssetRepository.GetClientAssetsByCodeAje(request.CodeAje);
            if (clientAssetValidation.Count != 0 && clientAssetById.CodeAje != request.CodeAje)
            {
                throw new InvalidOperationException($"Ya existe un Activo con el Código Aje '{request.CodeAje}' asociado a un cliente.");
            }

            if (!string.IsNullOrEmpty(request.CodeAje))
            {
                var assetByCode = await this._assetRepository.GetAssetByCodeAje(request.CodeAje);
                if (assetByCode.Count == 0)
                {
                    throw new InvalidOperationException($"No se encontró un Activo con el Código Aje '{request.CodeAje}'.");
                }

                if (assetByCode.FirstOrDefault() !.AssetId != request.AssetId)
                {
                    request.AssetId = assetByCode.FirstOrDefault() !.AssetId;
                }
            }

            var existingAnswer = await this._censusRepository.GetCensusAnswerAsync(
                request.CensusQuestionsId,
                request.ClientId,
                request.AssetId,
                monthPeriod);

            if (existingAnswer != null)
            {
                throw new InvalidOperationException("Ya se ha registrado una respuesta para esta pregunta en este período.");
            }

            var answer = request.Answer;

            if (request.ImageFile != null)
            {
                await using var stream = request.ImageFile.OpenReadStream();
                var documentName = request.ImageFile.FileName;
                var fileName = $"{documentName}";
                answer = await this._s3Service.UploadFileAsync(stream, "census-images", request.ClientId.ToString(), monthPeriod, fileName);
            }

            var censusAnswer = new CensusAnswer
            {
                CensusQuestionsId = request.CensusQuestionsId,
                Answer = answer,
                ClientId = request.ClientId,
                AssetId = request.AssetId,
                MonthPeriod = monthPeriod,
                CreatedAt = DateTime.Now,
                CreatedBy = request.CreatedBy,
            };

            await this._censusRepository.AddCensusAnswer(censusAnswer);
            var clientAssets = await this._clientAssetRepository.GetClientAssetByIdAsync(request.ClientAssetId);
            if (clientAssets.CodeAje != request.CodeAje)
            {
                clientAssets.AssetId = censusAnswer.AssetId;
                clientAssets.CodeAje = request.CodeAje;
                await this._clientAssetRepository.UpdateClientAssetsAsync(clientAssets);
            }

            return Unit.Value;
        }
    }
}
