namespace BackEndAje.Api.Application.Census.Commands.CreateCensusAnswer
{
    using BackEndAje.Api.Application.Behaviors;
    using MediatR;
    using Microsoft.AspNetCore.Http;

    public class CreateCensusAnswerCommand : IRequest<Unit>, IHasCreatedByInfo
    {
        public int ClientId { get; set; }

        public int CreatedBy { get; set; }

        public List<CreateCensusAnswerItem> Answers { get; set; } = new();
    }
}