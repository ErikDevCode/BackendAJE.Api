namespace BackEndAje.Api.Application.Dtos.Users
{
    using BackEndAje.Api.Application.Dtos.Cedi;
    using BackEndAje.Api.Application.Dtos.Zone;

    public class UserDto
    {
        public int UserId { get; set; }

        public int? CediId { get; set; }

        public CediDto Cedi { get; set; }

        public int? ZoneId { get; set; }

        public ZoneDto Zone { get; set; }

        public int? Route { get; set; }

        public int? Code { get; set; }

        public string PaternalSurName { get; set; }

        public string MaternalSurName { get; set; }

        public string Names { get; set; }

        public string? Email { get; set; }

        public string Phone { get; set; }

        public bool IsActive { get; set; }
    }
}
