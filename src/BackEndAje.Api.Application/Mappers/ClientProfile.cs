namespace BackEndAje.Api.Application.Mappers
{
    using AutoMapper;
    using BackEndAje.Api.Application.Clients.Commands.CreateClient;
    using BackEndAje.Api.Application.Clients.Queries.GetAllClients;
    using BackEndAje.Api.Application.Dtos.Cedi;
    using BackEndAje.Api.Application.Dtos.DocumentType;
    using BackEndAje.Api.Application.Dtos.PaymentMethod;
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

            this.CreateMap<Client, GetAllClientsResult>();

            this.CreateMap<User, UserDto>();

            this.CreateMap<Cedi, CediDto>();

            this.CreateMap<Zone, ZoneDto>();

            this.CreateMap<PaymentMethods, PaymentMethodDto>();

            this.CreateMap<DocumentType, DocumentTypeDto>();
        }
    }
}
