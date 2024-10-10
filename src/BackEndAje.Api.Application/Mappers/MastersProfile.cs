namespace BackEndAje.Api.Application.Mappers
{
    using AutoMapper;
    using BackEndAje.Api.Application.Masters.Queries.GetAllReasonRequest;
    using BackEndAje.Api.Application.Masters.Queries.GetWithDrawalReason;
    using BackEndAje.Api.Domain.Entities;

    public class MastersProfile : Profile
    {
        public MastersProfile()
        {
            this.CreateMap<ReasonRequest, GetAllReasonRequestResult>();

            this.CreateMap<WithDrawalReason, GetWithDrawalReasonResult>();
        }
    }
}
