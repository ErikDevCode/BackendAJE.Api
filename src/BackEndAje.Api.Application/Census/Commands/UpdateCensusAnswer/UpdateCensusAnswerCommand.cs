namespace BackEndAje.Api.Application.Census.Commands.UpdateCensusAnswer
{
    using BackEndAje.Api.Application.Behaviors;
    using MediatR;
    using Microsoft.AspNetCore.Http;

    public class UpdateCensusAnswerCommand: IRequest<Unit>, IHasCreatedByInfo
    {
        public int censusAnswerId { get; set; }

        public string? answer { get; set; }

        public int CreatedBy { get; set; }

        public IFormFile? ImageFile { get; set; }
    }
}
