namespace BackEndAje.Api.Application.Mappers
{
    using AutoMapper;
    using BackEndAje.Api.Application.Dtos.Cedi;
    using BackEndAje.Api.Application.Dtos.Client;
    using BackEndAje.Api.Application.Dtos.OrderRequests;
    using BackEndAje.Api.Application.Dtos.Product;
    using BackEndAje.Api.Application.Dtos.ReasonRequest;
    using BackEndAje.Api.Application.Dtos.TimeWindows;
    using BackEndAje.Api.Application.Dtos.Users;
    using BackEndAje.Api.Application.Dtos.WithDrawalReason;
    using BackEndAje.Api.Application.OrderRequestDocument.Queries.GetOrderRequestDocumentById;
    using BackEndAje.Api.Application.OrderRequests.Commands.CreateOrderRequests;
    using BackEndAje.Api.Application.OrderRequests.Documents.Commands.CreateDocumentByOrderRequest;
    using BackEndAje.Api.Application.OrderRequests.Documents.Commands.UpdateDocumentByOrderRequest;
    using BackEndAje.Api.Application.OrderRequests.Queries.GetAllOrderRequests;
    using BackEndAje.Api.Application.OrderRequests.Queries.GetOrderRequestById;
    using BackEndAje.Api.Application.OrderRequests.Queries.GetTrackingByOrderRequestId;
    using BackEndAje.Api.Domain.Entities;

    public class OrderRequestProfile : Profile
    {
        public OrderRequestProfile()
        {
            this.CreateMap<CreateOrderRequestsCommand, OrderRequest>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now));

            this.CreateMap<CreateOrderRequestDocumentDto, OrderRequestDocument>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now));

            this.CreateMap<CreateDocumentByOrderRequestCommand, OrderRequestDocument>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now));

            this.CreateMap<UpdateDocumentByOrderRequestCommand, OrderRequestDocument>()
                .ForMember(dest => dest.DocumentId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now));

            this.CreateMap<OrderRequest, GetOrderRequestByIdResult>()
                .ForMember(dest => dest.Client, opt => opt.MapFrom(src => src.Client))
                .ForMember(dest => dest.Supervisor, opt => opt.MapFrom(src => src.Supervisor))
                .ForMember(dest => dest.OrderRequestDocuments, opt => opt.MapFrom(src => src.OrderRequestDocuments));

            this.CreateMap<User, SupervisorDto>()
                .ForMember(
                    dest => dest.supervisorName,
                    opt => opt.MapFrom(src => $"{src.Names} {src.PaternalSurName} {src.MaternalSurName}"));

            this.CreateMap<Client, ClientDto>();
            this.CreateMap<Cedi, CediDto>();
            this.CreateMap<ReasonRequest, ReasonRequestDto>();
            this.CreateMap<TimeWindow, TimeWindowDto>();
            this.CreateMap<ProductType, ProductTypeDto>();
            this.CreateMap<Logo, LogoDto>();
            this.CreateMap<ProductSize, ProductSizeDto>();
            this.CreateMap<WithDrawalReason, WithDrawalReasonDto>();
            this.CreateMap<OrderRequestDocument, OrderRequestDocumentDto>();
            this.CreateMap<OrderRequestDocument, CreateOrderRequestDocumentDto>();
            this.CreateMap<OrderRequestDocument, GetOrderRequestDocumentByIdResult>();

            this.CreateMap<OrderRequest, GetAllOrderRequestsResult>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.Reason, opt => opt.MapFrom(src => src.ReasonRequest.ReasonDescription))
                .ForMember(dest => dest.Branch, opt => opt.MapFrom(src => src.Sucursal.CediName))
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Client.CompanyName))
                .ForMember(dest => dest.ClientCode, opt => opt.MapFrom(src => src.Client.ClientCode))
                .ForMember(dest => dest.Zone, opt => opt.MapFrom(src => src.Client.Seller!.Zone!.ZoneCode))
                .ForMember(dest => dest.Route, opt => opt.MapFrom(src => src.Client.Route))
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.OrderStatus.StatusName));

            this.CreateMap<OrderRequestStatusHistory, GetTrackingByOrderRequestIdResult>()
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.OrderStatus.StatusName))
                .ForMember(dest => dest.ResponsibleUser, opt => opt.MapFrom(src => $"{src.CreatedByUser .Names} {src.CreatedByUser .PaternalSurName} {src.CreatedByUser .MaternalSurName}"));
        }
    }
}
