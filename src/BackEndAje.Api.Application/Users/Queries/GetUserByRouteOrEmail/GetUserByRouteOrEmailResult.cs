namespace BackEndAje.Api.Application.Users.Queries.GetUserByRouteOrEmail
{
    public class GetUserByRouteOrEmailResult
    {
        public int UserId { get; set; }

        public int? CediId { get; set; }

        public string? CediName { get; set; }

        public int? ZoneId { get; set; }

        public int? ZoneCode { get; set; }

        public int? Route { get; set; }

        public int? Code { get; set; }

        public string PaternalSurName { get; set; }

        public string MaternalSurName { get; set; }

        public string Names { get; set; }

        public string UserName { get; set; }

        public string? Email { get; set; }

        public string Phone { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public GetUserByRouteOrEmailResult() { }
    }
}
