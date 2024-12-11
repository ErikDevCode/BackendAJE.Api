namespace BackEndAje.Api.Application.Census.Commands.UpdateCensusAnswer
{
    using BackEndAje.Api.Application.Interfaces;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class UpdateCensusAnswerHandler: IRequestHandler<UpdateCensusAnswerCommand, Unit>
    {
        private readonly ICensusRepository _censusRepository;
        private readonly IS3Service _s3Service;

        public UpdateCensusAnswerHandler(ICensusRepository censusRepository, IS3Service s3Service)
        {
            this._censusRepository = censusRepository;
            this._s3Service = s3Service;
        }

        public async Task<Unit> Handle(UpdateCensusAnswerCommand request, CancellationToken cancellationToken)
        {
            var censusAnswerDto = await this._censusRepository.GetCensusAnswerById(request.censusAnswerId);

            var answer = request.answer;

            if (request.ImageFile != null)
            {
                await using var stream = request.ImageFile.OpenReadStream();
                var documentName = request.ImageFile.FileName;
                var fileName = $"{documentName}";
                answer = await this._s3Service.UploadFileAsync(stream, "census-images", censusAnswerDto!.ClientId.ToString(), censusAnswerDto.MonthPeriod, fileName);
            }

            var censusAnswer = new CensusAnswer
            {
                CensusAnswerId = censusAnswerDto.CensusAnswerId,
                CensusQuestionsId = censusAnswerDto.CensusQuestionsId,
                Answer = answer,
                ClientId = censusAnswerDto.ClientId,
                AssetId = censusAnswerDto.AssetId,
                MonthPeriod = censusAnswerDto.MonthPeriod,
                CreatedAt = DateTime.Now,
                CreatedBy = request.CreatedBy,
            };

            await this._censusRepository.UpdateCensusAnswer(censusAnswer);
            return Unit.Value;
        }
    }
}
