namespace BackEndAje.Api.Application.Mappers
{
    using AutoMapper;
    using BackEndAje.Api.Application.Masters.Queries.GetAllDocumentType;
    using BackEndAje.Api.Application.Masters.Queries.GetAllLogos;
    using BackEndAje.Api.Application.Masters.Queries.GetAllPaymentMethod;
    using BackEndAje.Api.Application.Masters.Queries.GetAllProductSize;
    using BackEndAje.Api.Application.Masters.Queries.GetAllProductTypes;
    using BackEndAje.Api.Application.Masters.Queries.GetAllReasonRequest;
    using BackEndAje.Api.Application.Masters.Queries.GetAllTimeWindows;
    using BackEndAje.Api.Application.Masters.Queries.GetWithDrawalReason;
    using BackEndAje.Api.Domain.Entities;

    public class MastersProfile : Profile
    {
        public MastersProfile()
        {
            this.CreateMap<ReasonRequest, GetAllReasonRequestResult>();

            this.CreateMap<WithDrawalReason, GetWithDrawalReasonResult>();

            this.CreateMap<TimeWindow, GetAllTimeWindowsResult>();

            this.CreateMap<ProductType, GetAllProductTypesResult>();

            this.CreateMap<Logo, GetAllLogosResult>();

            this.CreateMap<ProductSize, GetAllProductSizeResult>();

            this.CreateMap<PaymentMethods, GetAllPaymentMethodResult>();

            this.CreateMap<DocumentType, GetAllDocumentTypeResult>();
        }
    }
}
