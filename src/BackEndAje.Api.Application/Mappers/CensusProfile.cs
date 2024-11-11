namespace BackEndAje.Api.Application.Mappers
{
    using AutoMapper;
    using BackEndAje.Api.Application.Census.Queries.GetCensusQuestions;
    using BackEndAje.Api.Domain.Entities;

    public class CensusProfile : Profile
    {
        public CensusProfile()
        {
            this.CreateMap<CensusQuestion, GetCensusQuestionsResult>();
        }
    }
}