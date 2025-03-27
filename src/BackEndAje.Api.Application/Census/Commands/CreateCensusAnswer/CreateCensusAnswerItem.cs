namespace BackEndAje.Api.Application.Census.Commands.CreateCensusAnswer
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    public class CreateCensusAnswerItem
    {
        public int? ClientAssetId { get; set; }

        public string? CodeAje { get; set; }

        public int CensusQuestionsId { get; set; }

        public string? Answer { get; set; }

        public IFormFile? ImageFile { get; set; }
    }
}