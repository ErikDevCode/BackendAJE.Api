namespace BackEndAje.Api.Application.Mappers
{
    using AutoMapper;
    using BackEndAje.Api.Application.Positions.Commands.CreatePosition;
    using BackEndAje.Api.Application.Positions.Commands.UpdatePosition;
    using BackEndAje.Api.Application.Positions.Queries.GetAllPositions;
    using BackEndAje.Api.Domain.Entities;

    public class PositionProfile : Profile
    {
        public PositionProfile()
        {
            this.CreateMap<Position, GetAllPositionsResult>();

            this.CreateMap<CreatePositionCommand, Position>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now));

            this.CreateMap<UpdatePositionCommand, Position>()
                .ForMember(dest => dest.PositionId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now));
        }
    }
}

