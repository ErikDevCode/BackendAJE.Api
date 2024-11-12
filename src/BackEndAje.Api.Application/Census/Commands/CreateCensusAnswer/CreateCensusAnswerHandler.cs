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

        public CreateCensusAnswerHandler(ICensusRepository censusRepository, IS3Service s3Service)
        {
            this._censusRepository = censusRepository;
            this._s3Service = s3Service;
        }

        public async Task<Unit> Handle(CreateCensusAnswerCommand request, CancellationToken cancellationToken)
        {
            var answer = request.Answer;
            var monthPeriod = DateTime.Now.ToString("yyyyMM");

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
            return Unit.Value;
        }
    }
}
