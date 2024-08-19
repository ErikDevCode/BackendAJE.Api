namespace BackEndAje.Api.Application.Dtos.Users
{
    public class UpdateUserPasswordDto
    {
        public string Email { get; set; }

        public string NewPassword { get; set; }
    }
}
