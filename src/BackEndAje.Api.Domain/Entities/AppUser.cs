namespace BackEndAje.Api.Domain.Entities
{
    public class AppUser : AuditableEntity
    {
        public int AppUserId { get; set; }
        public int UserId { get; set; }
        public string RouteOrEmail { get; set; }
        public string PasswordHash { get; set; }
        
        public void SetPassword(string password)
        {
            this.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
        }
        
        public bool ValidatePassword(string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, this.PasswordHash);
        }
    }
}
