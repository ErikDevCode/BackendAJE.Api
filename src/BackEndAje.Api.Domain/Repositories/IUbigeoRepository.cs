namespace BackEndAje.Api.Domain.Repositories
{
    using BackEndAje.Api.Domain.Entities;

    public interface IUbigeoRepository
    {
        Task<List<Department>> GetDepartments();

        Task<List<Province>> GetProvincesByDepartmentId(string departmentId);

        Task<List<District>> GetDistrictsByProvinceId(string provinceId);
    }
}
