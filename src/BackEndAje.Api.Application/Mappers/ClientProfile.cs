namespace BackEndAje.Api.Application.Mappers
{
    using AutoMapper;
    using BackEndAje.Api.Application.Clients.Commands.CreateClient;
    using BackEndAje.Api.Application.Clients.Commands.DisableClient;
    using BackEndAje.Api.Application.Clients.Commands.UpdateClient;
    using BackEndAje.Api.Application.Clients.Queries.GetAllClients;
    using BackEndAje.Api.Application.Clients.Queries.GetClientByClientCode;
    using BackEndAje.Api.Application.Dtos.Cedi;
    using BackEndAje.Api.Application.Dtos.DocumentType;
    using BackEndAje.Api.Application.Dtos.PaymentMethod;
    using BackEndAje.Api.Application.Dtos.Ubigeo;
    using BackEndAje.Api.Application.Dtos.Users;
    using BackEndAje.Api.Application.Dtos.Zone;
    using BackEndAje.Api.Domain.Entities;

    public class ClientProfile : Profile
    {
        public ClientProfile()
        {
            this.CreateMap<CreateClientCommand, Client>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now));

            this.CreateMap<Client, GetAllClientsResult>()
                .ForMember(dest => dest.DocumentType, opt => opt.MapFrom(src => src.DocumentType))
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod))
                .ForMember(dest => dest.Seller, opt => opt.MapFrom(src => src.Seller))
                .ForMember(dest => dest.District, opt => opt.MapFrom(src => src.District));

            this.CreateMap<DocumentType, DocumentTypeDto>();
            this.CreateMap<PaymentMethods, PaymentMethodDto>();
            this.CreateMap<User, UserDto>()
                .ForMember(dest => dest.Cedi, opt => opt.MapFrom(src => src.Cedi))
                .ForMember(dest => dest.Zone, opt => opt.MapFrom(src => src.Zone));
            this.CreateMap<Cedi, CediDto>();
            this.CreateMap<Zone, ZoneDto>();
            this.CreateMap<District, DistrictDto>();

            this.CreateMap<UpdateClientCommand, Client>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now));

            this.CreateMap<Client, GetClientByClientCodeResult>()
                .ForMember(dest => dest.DocumentType, opt => opt.MapFrom(src => src.DocumentType))
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod))
                .ForMember(dest => dest.Seller, opt => opt.MapFrom(src => src.Seller))
                .ForMember(dest => dest.District, opt => opt.MapFrom(src => src.District))
                .ForMember(dest => dest.ClientAssets, opt => opt.MapFrom(src => src.ClientAssets ?? new List<ClientAssets>()));

            this.CreateMap<ClientWithAssetDto, GetClientByClientCodeResult>()
                .ForMember(dest => dest.DocumentType, opt => opt.MapFrom(src => src.DocumentType))
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod))
                .ForMember(dest => dest.Seller, opt => opt.MapFrom(src => src.Seller))
                .ForMember(dest => dest.District, opt => opt.MapFrom(src => src.District))
                .ForMember(dest => dest.ClientAssets, opt => opt.MapFrom(src => src.ClientAssets ?? new List<ClientAssets>()));

            this.CreateMap<DisableClientCommand, Client>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now));
        }
    }
}
