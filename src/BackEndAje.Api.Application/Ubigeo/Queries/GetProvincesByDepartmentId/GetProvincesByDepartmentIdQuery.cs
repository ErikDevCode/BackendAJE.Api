namespace BackEndAje.Api.Application.Ubigeo.Queries.GetProvincesByDepartmentId
{
    using MediatR;

    public record GetProvincesByDepartmentIdQuery(string departmentId) : IRequest<List<GetProvincesByDepartmentIdResult>>
    {
    }
}
