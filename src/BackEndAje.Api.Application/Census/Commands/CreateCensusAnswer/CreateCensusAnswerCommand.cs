namespace BackEndAje.Api.Application.Census.Commands.CreateCensusAnswer
{
    using MediatR;
    using Microsoft.AspNetCore.Http;

    public class CreateCensusAnswerCommand : IRequest<Unit>
    {
        public int CensusQuestionsId { get; set; }

        public string? Answer { get; set; }

        public int ClientId { get; set; }

        public int AssetId { get; set; }

        public int CreatedBy { get; set; }

        public IFormFile? ImageFile { get; set; }
    }
}