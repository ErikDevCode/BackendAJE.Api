namespace BackEndAje.Api.Application.Users.Commands.UpdateUser
{
    using MediatR;

    public class UpdateUserCommand : IRequest<UpdateUserResult>
    {
        public int RegionId { get; set; }

        public int? CediId { get; set; }

        public int? ZoneId { get; set; }

        public int? Route { get; set; }

        public int? Code { get; set; }

        public string PaternalSurName { get; set; }

        public string MaternalSurName { get; set; }

        public string Names { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public int CreatedBy { get; set; }

        public int UpdatedBy { get; set; }
    }
}
