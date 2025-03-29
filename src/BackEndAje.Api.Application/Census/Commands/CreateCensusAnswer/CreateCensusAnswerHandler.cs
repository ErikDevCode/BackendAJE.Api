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
        private readonly IClientRepository _clientRepository;
        private readonly IUserRepository _userRepository;

        public CreateCensusAnswerHandler(ICensusRepository censusRepository, IS3Service s3Service, IClientAssetRepository clientAssetRepository, IAssetRepository assetRepository, IClientRepository clientRepository, IUserRepository userRepository)
        {
            this._censusRepository = censusRepository;
            this._s3Service = s3Service;
            this._clientAssetRepository = clientAssetRepository;
            this._assetRepository = assetRepository;
            this._clientRepository = clientRepository;
            this._userRepository = userRepository;
        }

        public async Task<Unit> Handle(CreateCensusAnswerCommand request, CancellationToken cancellationToken)
        {
            var monthPeriod = DateTime.Now.ToString("yyyyMM");

            var censusForm = await this._censusRepository.GetCensusFormAsync(request.ClientId, monthPeriod);

            if (censusForm == null)
            {
                try
                {
                    censusForm = await this._censusRepository.CreateCensusFormAsync(request.ClientId, monthPeriod);
                }
                catch (Exception)
                {
                    censusForm = await this._censusRepository.GetCensusFormAsync(request.ClientId, monthPeriod);
                }
            }

            foreach (var item in request.Answers)
            {
                // Si la primera pregunta es "No", eliminar clientAssets y censusAnswers relacionados
                if (item.CensusQuestionsId == 1 && string.Equals(item.Answer, "No", StringComparison.OrdinalIgnoreCase))
                {
                    if (request.Answers.Count > 1)
                        throw new InvalidOperationException("Si la respuesta es 'No', no debes enviar más de una respuesta.");

                    var alreadyExists = await this._censusRepository.GetCensusAnswerAsync(
                        item.CensusQuestionsId,
                        censusForm.CensusFormId,
                        null);

                    if (alreadyExists == null)
                    {
                        var census = new CensusAnswer
                        {
                            CensusQuestionsId = item.CensusQuestionsId,
                            Answer = item.Answer,
                            ClientAssetId = null,
                            CensusFormId = censusForm.CensusFormId,
                            CreatedAt = DateTime.Now,
                            CreatedBy = request.CreatedBy,
                        };

                        await this._censusRepository.AddCensusAnswer(census);
                    }

                    var clientAssets = await this._clientAssetRepository.GetClientAssetsByClientId(request.ClientId);

                    foreach (var ca in clientAssets)
                    {
                        if (ca.ClientAssetId > 0)
                        {
                            var relatedAnswers = await this._censusRepository.GetCensusAnswersByClientAssetIdAsync(ca.ClientAssetId);
                            foreach (var ans in relatedAnswers)
                            {
                                await this._censusRepository.DeleteCensusAnswerAsync(ans);
                            }
                        }

                        await this._clientAssetRepository.DeleteClientAssetAsync(ca);
                    }

                    return Unit.Value;
                }

                // Validación y creación de clientAsset si solo se envía codeAje
                if (item.ClientAssetId == null && !string.IsNullOrEmpty(item.CodeAje))
                {
                    var existingClientAssetsByCodeAje = await this._clientAssetRepository.GetClientAssetsByCodeAje(item.CodeAje);
                    if (existingClientAssetsByCodeAje.Any())
                    {
                        item.ClientAssetId = existingClientAssetsByCodeAje.First().ClientAssetId;
                    }
                    else
                    {
                        var matchingAssets = await this._assetRepository.GetAssetByCodeAje(item.CodeAje);
                        if (!matchingAssets.Any())
                            throw new InvalidOperationException($"No se encontró un activo con el código AJE '{item.CodeAje}'.");

                        var asset = matchingAssets.First();
                        var client = await this._clientRepository.GetClientById(request.ClientId);
                        var user = await this._userRepository.GetUserByRouteAsync(client.Route);

                        var newClientAsset = new ClientAssets
                        {
                            ClientId = request.ClientId,
                            AssetId = asset.AssetId,
                            CodeAje = item.CodeAje,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.Now,
                            CreatedBy = request.CreatedBy,
                            UpdatedBy = request.CreatedBy,
                            IsActive = true,
                            Notes = "Registro automático por censo",
                            CediId = user.CediId,
                            InstallationDate = DateTime.Now,
                        };

                        await this._clientAssetRepository.AddClientAsset(newClientAsset);
                        item.ClientAssetId = newClientAsset.ClientAssetId;
                    }
                }

                // Obtener ClientAsset si existe
                ClientAssets? clientAsset = null;
                if (item.ClientAssetId.HasValue)
                {
                    clientAsset = await this._clientAssetRepository.GetClientAssetByIdAsync(item.ClientAssetId.Value);
                }

                // Actualización del logo si aplica
                if (clientAsset != null && item.CensusQuestionsId == 7)
                {
                    var asset = await this._assetRepository.GetAssetById(clientAsset.AssetId!);
                    if (!string.Equals(asset.Logo?.Trim(), item.Answer?.Trim(), StringComparison.OrdinalIgnoreCase))
                    {
                        asset.Logo = item.Answer!;
                        await this._assetRepository.UpdateAssetAsync(asset);
                    }
                }

                // Validar si ya existe la respuesta
                var existingAnswer = await this._censusRepository.GetCensusAnswerAsync(
                    item.CensusQuestionsId,
                    censusForm.CensusFormId,
                    item.ClientAssetId);

                if (existingAnswer != null)
                    continue;

                var answer = item.Answer;
                if (item.ImageFile != null)
                {
                    await using var stream = item.ImageFile.OpenReadStream();
                    var fileName = item.ImageFile.FileName;
                    answer = await this._s3Service.UploadFileAsync(stream, "census-images", request.ClientId.ToString(), monthPeriod, fileName);
                }

                var censusAnswer = new CensusAnswer
                {
                    CensusQuestionsId = item.CensusQuestionsId,
                    Answer = answer,
                    ClientAssetId = item.ClientAssetId,
                    CensusFormId = censusForm.CensusFormId,
                    CreatedAt = DateTime.Now,
                    CreatedBy = request.CreatedBy,
                };

                await this._censusRepository.AddCensusAnswer(censusAnswer);
            }

            return Unit.Value;
        }
    }
}
