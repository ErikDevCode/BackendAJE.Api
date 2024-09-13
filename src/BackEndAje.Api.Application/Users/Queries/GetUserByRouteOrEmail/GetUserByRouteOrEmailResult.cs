namespace BackEndAje.Api.Application.Users.Queries.GetUserByRouteOrEmail
{
    public class GetUserByRouteIdOrEmailResult
    {
        public int UserId { get; }

        public int? RegionId { get; }

        public int? CediId { get; }

        public int? ZoneId { get; }

        public int? RouteId { get; }

        public int? Code { get; }

        public string PaternalSurName { get; }

        public string MaternalSurName { get; }

        public string Names { get; }

        public string Username { get; }

        public string Email { get; }

        public string Phone { get; }

        public bool IsActive { get; }

        public DateTime CreatedAt { get; }

        public GetUserByRouteIdOrEmailResult(int userId, int regionId, int? cediId, int? zoneId, int? routeId, int? code, string paternalSurName, string maternalSurName, string names, string username, string email, string phone, bool isActive, DateTime createdAt)
        {
            this.UserId = userId;
            this.RegionId = regionId;
            this.CediId = cediId;
            this.ZoneId = zoneId;
            this.RouteId = routeId;
            this.Code = code;
            this.PaternalSurName = paternalSurName;
            this.MaternalSurName = maternalSurName;
            this.Names = names;
            this.Username = username;
            this.Email = email;
            this.Phone = phone;
            this.IsActive = isActive;
            this.CreatedAt = createdAt;
        }
    }
}
