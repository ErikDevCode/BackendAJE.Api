namespace BackEndAje.Api.Application.Dtos.Users
{
    public class UpdateUserDto
    {
        public int? CediId { get; set; }

        public int? ZoneId { get; set; }

        public int? Route { get; set; }

        public int? Code { get; set; }

        public string PaternalSurName { get; set; }

        public string MaternalSurName { get; set; }

        public string Names { get; set; }

        public string? Email { get; set; }

        public string Phone { get; set; }

        public bool IsActive { get; set; }

        public int UpdatedBy { get; set; }
    }
}
