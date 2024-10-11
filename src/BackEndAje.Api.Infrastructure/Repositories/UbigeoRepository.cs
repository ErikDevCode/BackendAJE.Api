namespace BackEndAje.Api.Infrastructure.Repositories
{
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using BackEndAje.Api.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;

    public class UbigeoRepository : IUbigeoRepository
    {
        private readonly ApplicationDbContext _context;

        public UbigeoRepository(ApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<List<Department>> GetDepartments()
        {
            return await this._context.Departments.ToListAsync();
        }

        public async Task<List<Province>> GetProvincesByDepartmentId(string departmentId)
        {
            return await this._context.Provinces.Where(x => x.DepartmentId == departmentId).ToListAsync();
        }

        public async Task<List<District>> GetDistrictsByProvinceId(string provinceId)
        {
            return await this._context.Districts.Where(x => x.ProvinceId == provinceId).ToListAsync();
        }
    }
}
