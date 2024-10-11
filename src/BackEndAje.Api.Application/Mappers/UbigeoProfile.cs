namespace BackEndAje.Api.Application.Mappers
{
    using AutoMapper;
    using BackEndAje.Api.Application.Ubigeo.Queries.GetDepartments;
    using BackEndAje.Api.Application.Ubigeo.Queries.GetDistrictsByProvinceId;
    using BackEndAje.Api.Application.Ubigeo.Queries.GetProvincesByDepartmentId;
    using BackEndAje.Api.Domain.Entities;

    public class UbigeoProfile : Profile
    {
        public UbigeoProfile()
        {
            this.CreateMap<Department, GetDepartmentsResult>();

            this.CreateMap<Province, GetProvincesByDepartmentIdResult>();

            this.CreateMap<District, GetDistrictsByProvinceIdResult>();
        }
    }
}

