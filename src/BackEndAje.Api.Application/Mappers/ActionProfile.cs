namespace BackEndAje.Api.Application.Mappers
{
    using AutoMapper;
    using BackEndAje.Api.Application.Actions.Queries.GetAllActions;
    using BackEndAje.Api.Application.Dtos.Actions;
    using Action = BackEndAje.Api.Domain.Entities.Action;

    public class ActionProfile : Profile
    {
        public ActionProfile()
        {
            this.CreateMap<Action, GetAllActionsResult>();

            this.CreateMap<CreateActionDto, Action>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now));

            this.CreateMap<UpdateActionDto, Action>()
                .ForMember(dest => dest.ActionId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now));
        }
    }
}
