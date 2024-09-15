namespace BackEndAje.Api.Application.Users.Queries.GetUserByRouteOrEmail
{
    public class GetUserByRouteOrEmailResult
    {
        public int UserId { get; }

        public int? RegionId { get; }

        public string RegionName { get; set; }

        public int? CediId { get; }

        public string? CediName { get; set; }

        public int? ZoneId { get; }

        public int? ZoneCode { get; set; }

        public int? Route { get; }

        public int? Code { get; }

        public string PaternalSurName { get; }

        public string MaternalSurName { get; }

        public string Names { get; }

        public string Username { get; }

        public string? Email { get; }

        public string Phone { get; }

        public bool IsActive { get; }

        public DateTime CreatedAt { get; }

        public List<RoleResponse> Roles { get; set; } = new List<RoleResponse>();

        public GetUserByRouteOrEmailResult(int userId, int regionId, string regionName, int? cediId, string? cediName, int? zoneId, int? zoneCode, int? route, int? code,
            string paternalSurName, string maternalSurName, string names, string username, string? email, string phone, bool isActive, DateTime createdAt, List<RoleResponse> roles)
        {
            this.UserId = userId;
            this.RegionId = regionId;
            this.RegionName = regionName;
            this.CediId = cediId;
            this.CediName = cediName;
            this.ZoneId = zoneId;
            this.ZoneCode = zoneCode;
            this.Route = route;
            this.Code = code;
            this.PaternalSurName = paternalSurName;
            this.MaternalSurName = maternalSurName;
            this.Names = names;
            this.Username = username;
            this.Email = email;
            this.Phone = phone;
            this.IsActive = isActive;
            this.CreatedAt = createdAt;
            this.Roles = roles;
        }
    }
}
