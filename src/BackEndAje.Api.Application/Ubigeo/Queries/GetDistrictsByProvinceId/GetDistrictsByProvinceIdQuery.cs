namespace BackEndAje.Api.Application.Ubigeo.Queries.GetDistrictsByProvinceId
{
    using MediatR;

    public record GetDistrictsByProvinceIdQuery(string provinceId) : IRequest<List<GetDistrictsByProvinceIdResult>>
    {
    }
}
