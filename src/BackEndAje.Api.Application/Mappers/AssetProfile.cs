namespace BackEndAje.Api.Application.Mappers
{
    using AutoMapper;
    using BackEndAje.Api.Application.Asset.Command.CreateAsset;
    using BackEndAje.Api.Application.Asset.Command.CreateClientAsset;
    using BackEndAje.Api.Application.Asset.Command.UpdateAsset;
    using BackEndAje.Api.Application.Asset.Command.UpdateClientAsset;
    using BackEndAje.Api.Application.Asset.Queries.GetAllAssets;
    using BackEndAje.Api.Application.Asset.Queries.GetAssetsByCodeAje;
    using BackEndAje.Api.Application.Asset.Queries.GetAssetWithOutClient;
    using BackEndAje.Api.Application.Asset.Queries.GetClientAssets;
    using BackEndAje.Api.Application.Asset.Queries.GetClientAssetsTrace;
    using BackEndAje.Api.Domain.Entities;

    public class AssetProfile : Profile
    {
        public AssetProfile()
        {
            this.CreateMap<CreateAssetCommand, Asset>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now));

            this.CreateMap<Asset, GetAllAssetsResult>();

            this.CreateMap<UpdateAssetCommand, Asset>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now));

            this.CreateMap<Asset, GetAssetsByCodeAjeResult>();

            this.CreateMap<CreateClientAssetCommand, ClientAssets>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now));

            this.CreateMap<ClientAssetsDto, GetClientAssetsResult>();

            this.CreateMap<UpdateClientAssetCommand, ClientAssets>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now));

            this.CreateMap<ClientAssetsTrace, GetClientAssetsTraceResult>();

            this.CreateMap<Asset, GetAssetWithOutClientResult>();
        }
    }
}

